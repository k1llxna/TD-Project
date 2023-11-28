using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncherTurret;
    public TurretBlueprint laserTurret;
    public TurretBlueprint sniperTurret;
    public TurretBlueprint supportTurret;

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    // Start is called before the first frame update
    public void SelectBasicTurret()
    {
        buildManager.SelectTurretToBuild(standardTurret);
    }

    // Start is called before the first frame update
    public void SelectMissileTurret()
    {
        buildManager.SelectTurretToBuild(missileLauncherTurret);
    }

    public void SelectLaserTurret()
    {
        buildManager.SelectTurretToBuild(laserTurret);
    }

    public void SelectSniperTurret()
    {
        buildManager.SelectTurretToBuild(sniperTurret);
    }

    public void SelectSupportTurret()
    {
        buildManager.SelectTurretToBuild(supportTurret);
    }
}
