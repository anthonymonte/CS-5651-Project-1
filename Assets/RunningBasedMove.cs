using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class ActionBasedContinuousMoveProvider : ContinuousMoveProviderBase
    {
        [SerializeField]
        InputActionProperty m_LeftHandPositionAction = new InputActionProperty(new InputAction("Left Hand Position", expectedControlType: "Vector3"));
        public InputActionProperty leftHandPositionAction
        {
            get => m_LeftHandPositionAction;
            set => SetInputActionProperty(ref m_LeftHandPositionAction, value);
        }

        [SerializeField]
        InputActionProperty m_RightHandPositionAction = new InputActionProperty(new InputAction("Right Hand Position", expectedControlType: "Vector3"));
        public InputActionProperty rightHandPositionAction
        {
            get => m_RightHandPositionAction;
            set => SetInputActionProperty(ref m_RightHandPositionAction, value);
        }

        [SerializeField]
        InputActionProperty m_RightTriggerAction = new InputActionProperty(new InputAction("Right Trigger", expectedControlType: "Axis"));
        public InputActionProperty rightTriggerAction
        {
            get => m_RightTriggerAction;
            set => SetInputActionProperty(ref m_RightTriggerAction, value);
        }

        [SerializeField]
        float m_Acceleration = 2f;

        [SerializeField]
        float m_Deceleration = 2f;

        float m_CurrentSpeed = 0f;

        Vector3 m_PreviousLeftHandPosition;
        Vector3 m_PreviousRightHandPosition;
        bool m_IsFirstFrame = true;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnEnable()
        {
            m_LeftHandPositionAction.EnableDirectAction();
            m_RightHandPositionAction.EnableDirectAction();
            m_RightTriggerAction.EnableDirectAction();
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDisable()
        {
            m_LeftHandPositionAction.DisableDirectAction();
            m_RightHandPositionAction.DisableDirectAction();
            m_RightTriggerAction.DisableDirectAction();
        }

        /// <inheritdoc />
        protected override Vector2 ReadInput()
        {
            float triggerValue = m_RightTriggerAction.action?.ReadValue<float>() ?? 0f;
            bool isTriggerPressed = triggerValue > 0.1f;

            if (!isTriggerPressed)
            {
                m_IsFirstFrame = true;
                m_CurrentSpeed = 0f;
                return Vector2.zero;
            }

            float deltaTime = Time.deltaTime;
            Vector3 currentLeftHandPosition = m_LeftHandPositionAction.action?.ReadValue<Vector3>() ?? Vector3.zero;
            Vector3 currentRightHandPosition = m_RightHandPositionAction.action?.ReadValue<Vector3>() ?? Vector3.zero;

            if (m_IsFirstFrame)
            {
                m_PreviousLeftHandPosition = currentLeftHandPosition;
                m_PreviousRightHandPosition = currentRightHandPosition;
                m_IsFirstFrame = false;
                return Vector2.zero;
            }
            float leftHandVelocityY = (currentLeftHandPosition.y - m_PreviousLeftHandPosition.y) / deltaTime;
            float rightHandVelocityY = (currentRightHandPosition.y - m_PreviousRightHandPosition.y) / deltaTime;
            m_PreviousLeftHandPosition = currentLeftHandPosition;
            m_PreviousRightHandPosition = currentRightHandPosition;
            float velocityThreshold = 0.01f;
            if (Mathf.Abs(leftHandVelocityY) > velocityThreshold && Mathf.Abs(rightHandVelocityY) > velocityThreshold &&
                leftHandVelocityY * rightHandVelocityY < 0f)
            {
                m_CurrentSpeed += m_Acceleration * deltaTime;
                if (m_CurrentSpeed > moveSpeed)
                    m_CurrentSpeed = moveSpeed;
            }
            else
            {
                m_CurrentSpeed -= m_Deceleration * deltaTime;
                if (m_CurrentSpeed < 0f)
                    m_CurrentSpeed = 0f;
            }
            float normalizedSpeed = m_CurrentSpeed / moveSpeed;

            return new Vector2(0f, normalizedSpeed);
        }

        void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
        {
            if (Application.isPlaying)
                property.DisableDirectAction();

            property = value;

            if (Application.isPlaying && isActiveAndEnabled)
                property.EnableDirectAction();
        }
    }
}
