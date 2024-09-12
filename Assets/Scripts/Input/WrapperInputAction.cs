
public class WrapperInputAction
{
    private string _move = "Move";
    private string _dash = "Dash";
    private string _avoidance = "Avoidance";
    private string _attackLight = "AttackLight";
    private string _attackStrong = "AttackStrong";

    public string Move { get => _move; set => _move = value; }
    public string Dash { get => _dash; set => _dash = value; }
    public string Avoidance { get => _avoidance; set => _avoidance = value; }
    public string AttackLight { get => _attackLight; set => _attackLight = value; }
    public string AttackStrong { get => _attackStrong; set => _attackStrong = value; }
}