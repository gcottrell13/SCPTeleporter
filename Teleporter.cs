using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LightToy = Exiled.API.Features.Toys.Light;
using PrimitiveToy = Exiled.API.Features.Toys.Primitive;

namespace SCPTeleporter;

internal class Teleporter
{
    const float spinPerSecond = 360f;
    public int Id { get; private set; }
    public float Cooldown { get; }
    public DateTime TimeLastUsed { get; private set; }

    public bool Usable => (DateTime.Now - TimeLastUsed).TotalSeconds > Cooldown;

    PrimitiveToy Base;
    LightToy Light;
    PrimitiveToy SphereEffect;

    float baseRotation = 0f;


    public Teleporter(int id, float cooldown = 5f)
    {
        TimeLastUsed = DateTime.Now;
        Id = id;
        Cooldown = cooldown;
        Base = PrimitiveToy.Create(PrimitiveType.Cube, scale: new Vector3(0.1f, 0.1f, 1));
        Base.MovementSmoothing = 20;
        Base.Collidable = false;

        SphereEffect = PrimitiveToy.Create(PrimitiveType.Sphere, scale: new Vector3(1, 0.2f, 1));
        SphereEffect.Color = new Color(0f, 0f, 1f, 0.2f);
        SphereEffect.Collidable = false;

        //SphereEffect.AdminToyBase.transform.parent = Base.AdminToyBase.transform;
        SphereEffect.MovementSmoothing = 20;

        Light = LightToy.Create(position: new Vector3(0, 0.5f, 0));
        Light.AdminToyBase.transform.parent = Base.AdminToyBase.transform;
        Light.MovementSmoothing = 20;
        Light.Color = Color.blue;
    }

    public Vector3 Position
    {
        get { return Base.Position; }
        set { Base.Position = value; }
    }

    public void SetUsed()
    {
        TimeLastUsed = DateTime.Now;
    }


    public void Update(float dt)
    {
        double seconds = (double)DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000f;
        if (Usable)
        {
            SphereEffect.Position = new Vector3(0, (float)Math.Sin(seconds), 0) * 0.5f + Base.Position + Vector3.up * 0.6f;
            SphereEffect.Visible = true;

            Base.Rotation *= Quaternion.AngleAxis(dt * spinPerSecond, Vector3.up);
        }
        else
        {
            SphereEffect.Visible = false;

            Base.Rotation *= Quaternion.AngleAxis(spinPerSecond * dt * ((float)(DateTime.Now - TimeLastUsed).TotalSeconds / Cooldown), Vector3.up);
        }
    }


    public void Destroy()
    {
        Light.Destroy();
        SphereEffect.Destroy();
        Base.Destroy();
    }

}
