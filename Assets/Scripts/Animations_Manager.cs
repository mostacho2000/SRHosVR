using UnityEngine;

public class Animations_Manager : MonoBehaviour
{
    [Header("Animators")]
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;
    public string leftOpenTrigger = "Open";
    public string rightOpenTrigger = "Open";
    public string leftOpenState = "Door_A_Open";
    public string rightOpenState = "Door_B_Open";
    public string leftUnlockedParameter = "doorUnlocked";
    public string rightUnlockedParameter = "doorUnlocked";
    public bool useAnimatorBoolForOpen = true;

    [Header("Legacy Animation (optional)")]
    public Animation leftDoorAnimation;
    public Animation rightDoorAnimation;
    public string leftAnimationClipName = "Door_A_Open";
    public string rightAnimationClipName = "Door_B_Open";

    [Header("Door State")]
    public bool doorUnlocked = false;
    public bool isDoorOpen = false;

    public void UnlockDoor()
    {
        doorUnlocked = true;
        SetDoorAnimatorBool(true);
        Debug.Log("[Animations_Manager] Door unlocked.");
    }

    public void LockDoor()
    {
        doorUnlocked = false;
        SetDoorAnimatorBool(false);
        Debug.Log("[Animations_Manager] Door locked.");
    }

    public void OnDoorButtonPressed()
    {
        doorUnlocked = true;
        SetDoorAnimatorBool(true);

        if (isDoorOpen)
        {
            Debug.Log("[Animations_Manager] La puerta ya está abierta.");
            return;
        }

        if (useAnimatorBoolForOpen)
        {
            Debug.Log("[Animations_Manager] Se cambió doorUnlocked en los animators.");
            return;
        }

        OpenDoor();
    }

    public void OpenDoor()
    {
        bool played = false;

        if (leftDoorAnimator != null)
        {
            if (TryPlayAnimator(leftDoorAnimator, leftOpenTrigger, leftOpenState))
            {
                played = true;
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
            if (TryPlayAnimator(rightDoorAnimator, rightOpenTrigger, rightOpenState))
            {
                played = true;
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

    private bool TryPlayAnimator(Animator animator, string triggerName, string stateName)
    {
        if (!string.IsNullOrEmpty(triggerName) && HasTriggerParameter(animator, triggerName))
        {
            animator.SetTrigger(triggerName);
            return true;
        }

        if (!string.IsNullOrEmpty(stateName))
        {
            animator.Play(stateName);
            return true;
        }

        Debug.LogWarning($"[Animations_Manager] Animator '{animator.gameObject.name}' no tiene trigger '{triggerName}' y no se proporcionó estado válido.", this);
        return false;
    }

    private void SetDoorAnimatorBool(bool value)
    {
        if (leftDoorAnimator != null && !string.IsNullOrEmpty(leftUnlockedParameter))
        {
            leftDoorAnimator.SetBool(leftUnlockedParameter, value);
        }

        if (rightDoorAnimator != null && !string.IsNullOrEmpty(rightUnlockedParameter))
        {
            rightDoorAnimator.SetBool(rightUnlockedParameter, value);
        }
    }

    private bool HasTriggerParameter(Animator animator, string triggerName)
    {
        foreach (var parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger && parameter.name == triggerName)
            {
                return true;
            }
        }

        return false;
    }
}
