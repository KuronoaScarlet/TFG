using Random = UnityEngine.Random;

public class SimEntity
{
    public float hp;
    public float atk;
    public float def;
    
    protected float initialHp;

    public void Reset()
    {
        hp = initialHp;
    }
    
    public bool IsDead()
    {
        return hp <= 0;
    }
}

public class SimPlayer : SimEntity
{
    public SimPlayer(float h, float a, float d)
    {
        hp = initialHp = h;
        atk = a;
        def = d;
    }
}

public class SimEnemy : SimEntity
{
    public SimEnemy(float h, float a, float d)
    {
        hp = initialHp = h;
        atk = a;
        def = d;
    }
}

public class Simulator
{
    public readonly float enemyWins = 0;
    public readonly float playerWins = 0;

    public Simulator(Player player, Enemy enemy, int lvl, float rate, int simulations)
    {
        // Variables necesarias
        SimPlayer p = new SimPlayer(player.stats[lvl].x, player.stats[lvl].y, player.stats[lvl].z);
        
        // Crear el bucle del simulador
        SimEnemy e = new SimEnemy(enemy.stats[lvl].x, enemy.stats[lvl].y, enemy.stats[lvl].z);
        for (int j = 0; j < simulations; j++)
        {
            int whoBegins = Random.Range(0, 100);
            if (whoBegins < 100 - rate)
            {
                Combat(p, e);
            }
            else
            {
                Combat(e, p);
            }

            if (p.IsDead())
            {
                ++enemyWins;
            }
            else if(e.IsDead())
            {
                ++playerWins;
            }
                
            p.Reset();
            e.Reset();
        }
    }

    private void Combat(SimEntity firstAttacker, SimEntity secondAttacker)
    {
        do
        {
            // El atacante ataca primero
            secondAttacker.hp -= DealDamage(firstAttacker.atk, secondAttacker.def);
            // Comprobar si el defensor ha muerto antes de su ataque 
            if (!secondAttacker.IsDead())
            {
                // El defensor ataca
                firstAttacker.hp -= DealDamage(secondAttacker.atk, firstAttacker.def);
            }
            // Comprobar si el atacante o el defensor ha muerto para salir del bucle
        } while (!firstAttacker.IsDead() && !secondAttacker.IsDead());
    }

    private float DealDamage(float attackerAtk, float defenderDef)
    {
        // Crear una fórmula de daño simple
        if (attackerAtk >= defenderDef)
        {
            return attackerAtk * 2 - defenderDef;
        }
        
        return attackerAtk * attackerAtk / defenderDef;
    }
}