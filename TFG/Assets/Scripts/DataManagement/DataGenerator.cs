using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SimulationState
{
    NewLevel,
    UpperStats,
    LowerStats
}

public class DataGenerator
{
    // Constants
    private const int MaxLevel = 100;
    private const float Lambda = 35f;

    // Entities
    public Player player = new Player();
    private Enemy _enemy;
    
    // Lists
    private readonly List<float> _slope = new List<float>();
    public readonly List<Enemy> enemies = new List<Enemy>();
    private readonly List<float> _enemyDesiredWinrate = new List<float>();
    private List<Vector2> _winRate = new List<Vector2>();
    public readonly List<List<Vector2>> winRateList = new List<List<Vector2>>();
    public readonly List<EntityClass> entityClassesList = new List<EntityClass>();
    
    // General variables
    private float _growthCoeficient;

    // Foreign instances
    private readonly Vector3 _initialStats = new Vector3();
    private PlayerGrowthState _playerGrowthState;
    private EntityClass _enemyToCreate;
    
    public DataGenerator(Vector3 stats)
    {
        _initialStats = stats;
    }

    public void SetEnemyType(EntityClass newEnemyClass)
    {
        _enemyToCreate = newEnemyClass;
    }

    public void SetPGS(PlayerGrowthState newPGS)
    {
        _playerGrowthState = newPGS;
    }

    private void SaveEntities()
    {
        // Guardamos los enemigos de manera preventiva antes de exportarlos.
        // Solamente guardamos 1 de cada clase, para evitar que haya duplicados.
        if (enemies.Count == 0)
        {
            enemies.Add(_enemy);
            
            List<Vector2> newWinRate = new List<Vector2>(_winRate);
            winRateList.Add(newWinRate);
            _winRate.Clear();
            
            entityClassesList.Add(_enemyToCreate);
        }
        else
        {
            int index = enemies.FindIndex(enemy => enemy.GetClass() == _enemy.GetClass());

            if (index != -1)
            {
                enemies[index].stats.Clear();
                enemies[index].stats = new List<Vector3>(_enemy.stats);

                winRateList[index].Clear();
                List<Vector2> newWinRate = new List<Vector2>(_winRate);
                winRateList[index] = new List<Vector2>(newWinRate);
                _winRate.Clear();
            }
            else
            {
                enemies.Add(_enemy);

                List<Vector2> newWinRate = new List<Vector2>(_winRate);
                winRateList.Add(newWinRate);
                _winRate.Clear();

                entityClassesList.Add(_enemyToCreate);
            }
        }

        Debug.Log("Data generated correctly!");
    }

    public void GenerateData(int simulations)
    {
        if (_playerGrowthState == PlayerGrowthState.GenStats)
        {
            // Se crea el player
            player.stats.Clear();
            player.AddInitialStats(_initialStats);
            _growthCoeficient = Random.Range(1, 9) / 10f;
        }

        _enemy = new Enemy(_initialStats, _enemyToCreate);
        
        // Cogemos el pendiente en todos los puntos de la curva de Bezier (Curva de Dificultad)
        DifficultySlopeGetter();

        // Obtener coeficientes de Winrate del player en función del Slope
        GetSlopeCoeficient();

        SimulationState state = SimulationState.NewLevel;
        
        for (int i = 0; i < MaxLevel; i++)
        {
            if (i != 0)
            {
                if (_playerGrowthState == PlayerGrowthState.GenStats)
                {
                    // Player Stats Growth
                    player.stats.Add(StatLevelGrower(player.stats[0], i));
                }

                // Enemies Stat Growth

                _enemy.stats.Add(StatLevelGrower(_enemy.stats[0], i));
            }

            do
            {
                if (state == SimulationState.UpperStats)
                {
                    // Función para hacer crecer stats.
                    _enemy.stats[i] = StatUpper(_enemy.stats[i]);
                }
                else if (state == SimulationState.LowerStats)
                {
                    // Función para decrecer stats.
                    _enemy.stats[i] = StatLower(_enemy.stats[i]);
                }

                // Simulamos combate en ese lvl.
                Simulator simulator = new Simulator(player, _enemy, i, _enemyDesiredWinrate[i], simulations);

                RoundUpEnemyStats();

                Vector2 lvlWinrate = GetWinRate(simulator.playerWins, simulator.enemyWins, simulations);

                if (!CheckWinRates(lvlWinrate.y, _enemyDesiredWinrate[i]))
                {
                    if (lvlWinrate.y < _enemyDesiredWinrate[i])
                    {
                        state = SimulationState.UpperStats;
                    }
                    else
                    {
                        state = SimulationState.LowerStats;
                    }
                }
                else
                {
                    _winRate.Add(lvlWinrate);
                    state = SimulationState.NewLevel;
                }
            } while (state != SimulationState.NewLevel);
        }
        
        SaveEntities();
    }

    private void RoundUpEnemyStats()
    {
        for (int i = 0; i < _enemy.stats.Count; i++)
        {
            var enemyStat = _enemy.stats[i];
            enemyStat.x = (float)Math.Round(enemyStat.x, 1);
            enemyStat.y = (float)Math.Round(enemyStat.y, 1);
            enemyStat.z = (float)Math.Round(enemyStat.z, 1);
            _enemy.stats[i] = enemyStat;
        }
    }

    public void ExportCall()
    {
        ExportCsvStatFiles(player, enemies);
        ExportTxtSimulationFile();
    }

    private void ExportTxtSimulationFile()
    {
        Debug.Log("Exporting Simulation Results");

        IList<string> finalSimulationData = new List<string>();

        for (int i = 0; i < winRateList.Count; ++i)
        {
            finalSimulationData.Add("Enemy " + enemies[i].GetClass() + " Win Rate:");
            int lvl = 1;

            for (int j = 0; j < winRateList[i].Count; j++)
            {
                finalSimulationData.Add("Lvl: " + lvl + " / Player Win Rate: " + winRateList[i][j].x + "%" + " / Enemy Win Rate: " + winRateList[i][j].y + "%");
                lvl++;
            }
            
            finalSimulationData.Add("\n");
        }

        string filePath = Application.dataPath + "/Exports/Simulation.txt";

        StreamWriter sw = new StreamWriter(filePath);

        foreach (var t in finalSimulationData)
        {
            sw.WriteLine(t);
        }
        
        sw.Close();
    }
    
    private static void ExportCsvStatFiles(Player player, List<Enemy> enemies)
    {
        player.FillFinalData();
        CSVExporter playerExporter = new CSVExporter(player.finalData, player.GetEntity());
        playerExporter.Export();

        foreach (var e in enemies)
        {
            e.FillFinalData();
            CSVExporter enemyExporter = new CSVExporter(e.finalData, e.GetEntity(), e.GetClass());
            enemyExporter.Export();
        }
    }

    private Vector3 StatUpper(Vector3 lvlStats)
    {
        return new Vector3(lvlStats.x + lvlStats.x * 0.1f, lvlStats.y + lvlStats.y * 0.1f, lvlStats.z + lvlStats.z * 0.1f);
    }
    
    private Vector3 StatLower(Vector3 lvlStats)
    {
        return new Vector3(lvlStats.x - lvlStats.x * 0.1f , lvlStats.y - lvlStats.y * 0.1f , lvlStats.z - lvlStats.z * 0.1f);
    }

    private Vector3 StatLevelGrower(Vector3 iStats, int lvl)
    {
        // Sujeto a cambios (Maybe guardar en una variable el coeficiente de crecimiento usado?)
        return new Vector3((float)Math.Round(iStats.x + iStats.x * _growthCoeficient * lvl, 1), (float)Math.Round(iStats.y + iStats.y * _growthCoeficient * lvl, 1), (float)Math.Round(iStats.z + iStats.z * _growthCoeficient * lvl, 1));
    }
    
    private void GetSlopeCoeficient()
    {
        _enemyDesiredWinrate.Clear();
        
        float defaultWinrate = 50.0f;

        foreach (var t in _slope)
        {
            _enemyDesiredWinrate.Add(defaultWinrate + Lambda - CoeficientFunction(t));
        }
        _enemyDesiredWinrate.Add(defaultWinrate + Lambda - CoeficientFunction(_slope[98]));
    }

    private float CoeficientFunction(float t)
    {
        // y = e^(-x) * 2 * l 
        return Mathf.Exp(-t) * 2 * Lambda;
    }
    
    public float CoeficientInverseFunction(float w)
    {
        var a = 50f + Lambda - w;
        if (a == 0)
        {
            a += 0.000015f;
        }
        
        // x = -ln(y/(2l))
        var z = a / (2 * Lambda);
        var coeficientInverseFunction = -Mathf.Log(Mathf.Abs(z));
        
        return coeficientInverseFunction;
    }

    private void DifficultySlopeGetter()
    {
        LineRenderer lineRenderer = GameObject.Find("Manager").GetComponent<LineRenderer>();
        Vector3[] linePositions = new Vector3[lineRenderer.positionCount];

        lineRenderer.GetPositions(linePositions);
        
        _slope.Clear();
        
        for (int i = 0; i < linePositions.Length - 1; i++)
        {
            float numerator = linePositions[i + 1].y - linePositions[i].y;
            float denominator = linePositions[i + 1].x - linePositions[i].x;
            float result = numerator / denominator;

            _slope.Add(result);
        }
    }

    private Vector2 GetWinRate(float pW, float eW, float simNum)
    {
        return new Vector2(pW / simNum * 100.0f, eW / simNum * 100.0f);
    }
    
    private bool CheckWinRates(float eWr, float dWr)
    {
        return Mathf.Abs(eWr - dWr) <= 1.25f;
    }
}