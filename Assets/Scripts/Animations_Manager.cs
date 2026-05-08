using UnityEngine;

public class Animations_Manager : MonoBehaviour
{
    [Header("Animators")]
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;
    public string leftOpenTrigger = "Open";
    public string rightOpenTrigger = "Open";
    public string leftOpenState = "Open";
    public string rightOpenState = "Open";
    public bool useAnimatorTrigger = true;

    [Header("Legacy Animation (optional)")]
    public Animation leftDoorAnimation;
    public Animation rightDoorAnimation;
    public string leftAnimationClipName = "Open";
    public string rightAnimationClipName = "Open";

    [Header("Door State")]
    public bool canOpenDoor = false;
    public bool isDoorOpen = false;

    public void AllowDoorOpen(bool allow)
    {
        canOpenDoor = allow;
    }

    public void OnDoorButtonPressed()
    {
        if (!canOpenDoor)
        {
            Debug.Log("[Animations_Manager] La puerta no está lista para abrir.");
            return;
        }

        if (isDoorOpen)
        {
            Debug.Log("[Animations_Manager] La puerta ya está abierta.");
            return;
        }

        OpenDoor();
    }

    public void OpenDoor()
    {
        bool played = false;

        if (leftDoorAnimator != null)
        {
            if (useAnimatorTrigger && !string.IsNullOrEmpty(leftOpenTrigger))
            {
                leftDoorAnimator.SetTrigger(leftOpenTrigger);
                played = true;
            }
            else if (!string.IsNullOrEmpty(leftOpenState))
            {
                leftDoorAnimator.Play(leftOpenState);
                played = true;
            }
            else
            {
                Debug.LogWarning("[Animations_Manager] leftDoorAnimator asignado pero no se proporcionó trigger ni estado.", this);
            }
        }
        else if (leftDoorAnimation != null)
        {
            if (!string.IsNullOrEmpty(leftAnimationClipName))
            {
                leftDoorAnimation.Play(leftAnimationClipName);
                played = true;
            }
            else
            {
                Debug.LogWarning("[Animations_Manager] leftDoorAnimation asignado pero leftAnimationClipName está vacío.", this);
            }
        }
        else
        {
            Debug.LogWarning("[Animations_Manager] leftDoorAnimator y leftDoorAnimation no están asignados.", this);
        }

        if (rightDoorAnimator != null)
        {
            if (useAnimatorTrigger && !string.IsNullOrEmpty(rightOpenTrigger))
            {
                rightDoorAnimator.SetTrigger(rightOpenTrigger);
                played = true;
            }
            else if (!string.IsNullOrEmpty(rightOpenState))
            {
                rightDoorAnimator.Play(rightOpenState);
                played = true;
            }
            else
            {
                Debug.LogWarning("[Animations_Manager] rightDoorAnimator asignado pero no se proporcionó trigger ni estado.", this);
            }
        }
        else if (rightDoorAnimation != null)
        {
            if (!string.IsNullOrEmpty(rightAnimationClipName))
            {
                rightDoorAnimation.Play(rightAnimationClipName);
                played = true;
            }
            else
            {
                Debug.LogWarning("[Animations_Manager] rightDoorAnimation asignado pero rightAnimationClipName está vacío.", this);
            }
        }
        else
        {
            Debug.LogWarning("[Animations_Manager] rightDoorAnimator y rightDoorAnimation no están asignados.", this);
        }

        if (played)
        {
            isDoorOpen = true;
            Debug.Log("[Animations_Manager] Puerta abierta correctamente.");
        }
        else
        {
            Debug.LogWarning("[Animations_Manager] No se pudo abrir la puerta porque no se inició ninguna animación.", this);
        }
    }
}
