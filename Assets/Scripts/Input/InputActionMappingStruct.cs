
public struct InputActionMappingStruct
{
    public InputActionTypeEnum ActionType;
    public string ActionName;

    public InputActionMappingStruct(InputActionTypeEnum actionType, string actionName)
    {
        ActionType = actionType;
        ActionName = actionName;
    }
}