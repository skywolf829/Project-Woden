using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerV2))]
public class PlayerResources : MonoBehaviour
{
    PlayerV2 p;

    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rb;
    public GameObject HealthBar;
    public GameObject ManaBar;
    string lastType = "";

    public float healthRegen, manaRegen;

    GameObject healthBar, manaBar;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        p = GetComponent<PlayerV2>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        p.health = p.maxHealth = 250;
        p.mana = p.maxMana = 100;

        healthBar = Instantiate(HealthBar);
        manaBar = Instantiate(ManaBar);
    }

    public void hitBy(string t)
    {
        lastType = t;
    }
    public void applyDamage(int d)
    {
        p.health -= d;
        if (p.health < 0)
        {
            p.health = 0;
            TriggerDeath();
        }
    }

    public IEnumerator UpdateResources()
    {
        p.health += (Time.time - p.lastUpdateTime) * healthRegen;
        p.health = Mathf.Min(p.maxHealth, p.health);
        p.mana += (Time.time - p.lastUpdateTime) * manaRegen;
        p.mana = Mathf.Min(p.maxMana, p.mana);
        updateHealthBar();
        updateManaBar();
        yield return null;
    }

    private void updateHealthBar()
    {
        Vector2 healthBarPos = transform.GetChild(1).position;
        healthBarPos += new Vector2(-(((1 - (float)p.health / p.maxHealth)) / 2), 0);
        Vector3 healthBarScale = new Vector3((float)p.health / p.maxHealth, 1f, 1);

        healthBar.transform.position = healthBarPos;
        healthBar.transform.localScale = healthBarScale;
    }
    private void updateManaBar()
    {
        Vector2 manaBarPos = transform.GetChild(2).position;
        manaBarPos += new Vector2(-(((1 - (float)p.mana / p.maxMana)) / 2), 0);
        Vector3 manaBarScale = new Vector3((float)p.mana / p.maxMana, 1f, 1);

        manaBar.transform.position = manaBarPos;
        manaBar.transform.localScale = manaBarScale;
    }
    void TriggerDeath()
    {

    }
}
