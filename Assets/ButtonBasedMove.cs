using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit
{
    [HelpURL("https://docs.unity3d.com/Manual/xr_input.html")]
    public class ButtonBasedContinuousMoveProvider : ContinuousMoveProviderBase
    {
        [SerializeField]
        InputActionProperty m_ForwardMoveAction = new InputActionProperty(new InputAction("Move Forward", expectedControlType: "Button"));
        public InputActionProperty forwardMoveAction
        {
            get => m_ForwardMoveAction;
            set => SetInputActionProperty(ref m_ForwardMoveAction, value);
        }

        [SerializeField]
        InputActionProperty m_BackwardMoveAction = new InputActionProperty(new InputAction("Move Backward", expectedControlType: "Button"));
        public InputActionProperty backwardMoveAction
        {
            get => m_BackwardMoveAction;
            set => SetInputActionProperty(ref m_BackwardMoveAction, value);
        }

        [SerializeField]
        float m_MaxForwardSpeed = 100f;

        [SerializeField]
        float m_MaxReverseSpeed = 3f;

        [SerializeField]
        float m_Acceleration = 20f;

        [SerializeField]
        float m_Deceleration = 100f;

        float m_CurrentSpeed = 0f;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnEnable()
        {
            m_ForwardMoveAction.EnableDirectAction();
            m_BackwardMoveAction.EnableDirectAction();
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDisable()
        {
            m_ForwardMoveAction.DisableDirectAction();
            m_BackwardMoveAction.DisableDirectAction();
        }

        /// <inheritdoc />
        protected override Vector2 ReadInput()
        {
            float forwardValue = m_ForwardMoveAction.action?.ReadValue<float>() ?? 0f;
            float backwardValue = m_BackwardMoveAction.action?.ReadValue<float>() ?? 0f;
            float deltaTime = Time.deltaTime;

            if (forwardValue > 0f)
            {
                if (m_CurrentSpeed >= 0f)
                {
                    m_CurrentSpeed += m_Acceleration * deltaTime;
                    if (m_CurrentSpeed > m_MaxForwardSpeed)
                        m_CurrentSpeed = m_MaxForwardSpeed;
                }
                else
                {
                    m_CurrentSpeed += m_Deceleration * deltaTime;
                    if (m_CurrentSpeed > 0f)
                        m_CurrentSpeed = 0f;
                }
            }
            else if (backwardValue > 0f)
            {
                if (m_CurrentSpeed <= 0f)
                {
                    m_CurrentSpeed -= m_Acceleration * deltaTime;
                    if (m_CurrentSpeed < -m_MaxReverseSpeed)
                        m_CurrentSpeed = -m_MaxReverseSpeed;
                }
                else
                {
                    m_CurrentSpeed -= m_Deceleration * deltaTime;
                    if (m_CurrentSpeed < 0f)
                        m_CurrentSpeed = 0f;
                }
            }
            else
            {
                if (m_CurrentSpeed > 0f)
                {
                    m_CurrentSpeed -= m_Deceleration * deltaTime;
                    if (m_CurrentSpeed < 0f)
                        m_CurrentSpeed = 0f;
                }
                else if (m_CurrentSpeed < 0f)
                {
                    m_CurrentSpeed += m_Deceleration * deltaTime;
                    if (m_CurrentSpeed > 0f)
                        m_CurrentSpeed = 0f;
                }
            }
            return new Vector2(0f, m_CurrentSpeed);
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
