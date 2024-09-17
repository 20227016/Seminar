using UnityEngine;

public class InputActionManager : MonoBehaviour
{
    private static readonly InputActionMappingStruct[] ActionMappings = new[]
    {
        new InputActionMappingStruct(InputActionTypeEnum.Move, "Move"),
        new InputActionMappingStruct(InputActionTypeEnum.Dash, "Dash"),
        new InputActionMappingStruct(InputActionTypeEnum.AttackLight, "AttackLight"),
        new InputActionMappingStruct(InputActionTypeEnum.AttackStrong, "AttackStrong"),
        new InputActionMappingStruct(InputActionTypeEnum.Avoidance, "Avoidance"),
        new InputActionMappingStruct(InputActionTypeEnum.Target, "Target"),
        new InputActionMappingStruct(InputActionTypeEnum.Skill, "Skill")
    };

    public static InputActionTypeEnum? GetActionType(string actionName)
    {
        foreach (var mapping in ActionMappings)
        {
            if (mapping.ActionName == actionName)
            {
                return mapping.ActionType;
            }
        }
        return null;
    }
}