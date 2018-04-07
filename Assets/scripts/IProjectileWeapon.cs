using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileWeapon {

    Transform projectileSpawn { get; set; }
    void castProjectile();


}
