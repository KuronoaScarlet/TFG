using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MainSceneManager : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField healthInput;
    public TMP_InputField attackInput;
    public TMP_InputField defenseInput;
    public TMP_InputField simNumInput;
    
    [Header("Toggles")]
    public Toggle fixToggle;
    
    [Header("Dropdowns")]
    public Dropdown enemyDropdown;
    public NewEnemyDropdown newEnemyDropdown;

    [Header("Draggable Objects")]
    public DraggableObject startTangent;
    public DraggableObject endTangent;

    [Header("GameObjects")]
    public GameObject firstGenButton;
    public GameObject exportButton;
    public GameObject resetButton;
    public GameObject playerCanvas;
    public GameObject enemiesCanvas;
    public GameObject statsChart;
    public GameObject rightButton;
    public GameObject leftButton;
    public GameObject enemyScrollView;
    public GameObject newEnemyGenButton;
    public GameObject regenerateEnemyButton;
    public GameObject openFolderButton;
    public GameObject outputCurveRenderer;
    public GameObject curveStartPoint;
    public GameObject curveEndPoint;

    [Header("TextBoxes")] 
    public TMP_Text playerStats;
    public TMP_Text enemyType;
    public TMP_Text enemyStatsHeader;
    public TMP_Text enemyStats;
    public TMP_Text exportText;

    [Header("The Data Generator")]
    private DataGenerator _dataGenerator;

    [Header("Extra Variables")]
    public PlayerGrowthState pGs;
    private bool _firstGen = true;
    private int _index = 0;

    private readonly float _fadeDuration = 3f;

    private void Start()
    {
        simNumInput.text = "5000";

        fixToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }
    
    #region Buttons

    public void GenerateButton()
    {
        if (enemyDropdown.enemyType != EntityClass.None)
        {
            if (_firstGen)
            {
                _dataGenerator = new DataGenerator(new Vector3(float.Parse(healthInput.text), float.Parse(attackInput.text), float.Parse(defenseInput.text)));
                _firstGen = false;
            }
            
            _dataGenerator.SetEnemyType(enemyDropdown.enemyType);
            _dataGenerator.SetPGS(pGs);
            _dataGenerator.GenerateData(int.Parse(simNumInput.text));
            
            ChangeView(true);

            FillPlayerStats();
            FillEnemyStats();
        }
        else
        {
            Debug.LogWarning("EnemyType not selected! Can't develop the enemy stats!");
        }
    }

    public void NewEnemyGenerateButton()
    {
        _dataGenerator.SetEnemyType(newEnemyDropdown.enemyType);
        _dataGenerator.SetPGS(pGs);
        _dataGenerator.GenerateData(int.Parse(simNumInput.text));
        
        FillPlayerStats();
        FillEnemyStats();
        
        if(_index != 3)rightButton.SetActive(true);
    }

    public void RegenerateEnemyButton()
    {
        _dataGenerator.SetEnemyType(_dataGenerator.entityClassesList[_index]);
        _dataGenerator.SetPGS(pGs);
        _dataGenerator.GenerateData(int.Parse(simNumInput.text));
        
        FillPlayerStats();
        FillEnemyStats();
    }
    
    public void RightButton()
    {
        _index++;
        if (_index == 3)
        {
            rightButton.SetActive(false);
        }
        leftButton.SetActive(true);

        FillChart();
    }
    
    public void LeftButton()
    {
        _index--;
        if (_index == 0)
        {
            leftButton.SetActive(false);
        }
        rightButton.SetActive(true);
        
        FillChart();
    }

    public void ExportButton()
    {
        _dataGenerator.ExportCall();

        exportText.gameObject.SetActive(true);
        exportText.text = $"Exported on {Application.dataPath}/Exports";
        exportText.alpha = 255;

        StartCoroutine(ExportTextFade());
        
        openFolderButton.SetActive(true);
    }

    private IEnumerator ExportTextFade()
    {
        Color originalColor = Color.white;
        Color transparentColor = new Color(255f, 255f, 255f, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / _fadeDuration);
            exportText.color = Color.Lerp(originalColor, transparentColor, alpha);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        exportText.color = transparentColor;
    }

    public void ResetButton()
    {
        ChangeView(false);
        
        _dataGenerator.player.stats.Clear();
        _dataGenerator.enemies.Clear();

        _firstGen = true;
        
        openFolderButton.SetActive(false);

        healthInput.text = "";
        attackInput.text = "";
        defenseInput.text = "";
        
        enemyDropdown.GetComponent<TMP_Dropdown>().value = 0;
        
        startTangent.ResetPosition();
        endTangent.ResetPosition();
    }

    public void OpenFolderButton()
    {
        string appPath = Application.dataPath.Replace("/", "\\");
        string filePath = $"{appPath}\\Exports";
        Process.Start("explorer.exe", filePath);
    }
    
    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            pGs = PlayerGrowthState.FixStats;
        }
        else
        {
            pGs = PlayerGrowthState.GenStats;
        }
    }

    #endregion

    #region StatChartsMethods

    public void CreateOutputCurve()
    {
        OutputCurveRenderer OCR = outputCurveRenderer.GetComponent<OutputCurveRenderer>();

        List<Vector3> curvePositions = new List<Vector3>();
        List<Vector2> indexWinrate = new List<Vector2>(_dataGenerator.winRateList[_index]);
        List<float> slopes = new List<float>();
        Vector3 endPoint = curveEndPoint.transform.position;
        Vector3 startPoint = curveStartPoint.transform.position;

        for (int i = 0; i < indexWinrate.Count; i++)
        {
            slopes.Add(_dataGenerator.CoeficientInverseFunction(indexWinrate[i].y));
        }
        
        float xDelta = (endPoint.x - startPoint.x) / 65;
        float yDelta = (endPoint.y - startPoint.y) / 60;

        Vector3 currentPoint = startPoint;
        curvePositions.Add(currentPoint);

        for (int i = 1; i < slopes.Count - 1; i++)
        {
            float slope = slopes[i];

            var alpha = Mathf.Atan(slope);

            var newX = currentPoint.x + xDelta * Mathf.Cos(alpha);
            var newy = currentPoint.y + yDelta * Mathf.Sin(alpha);

            var point = new Vector3(newX, newy, 90f);
            curvePositions.Add(point);

            currentPoint = point;
        }
        
        curvePositions.Add(endPoint);

        OCR.SetCurvePositions(curvePositions);
    }

    private void ChangeView(bool change)
    {
        exportButton.SetActive(change);
        fixToggle.gameObject.SetActive(change);
        firstGenButton.SetActive(!change);
        playerCanvas.SetActive(!change);
        enemiesCanvas.SetActive(!change);

        statsChart.SetActive(change);
        leftButton.SetActive(!change);
    }

    private void FillPlayerStats()
    {
        playerStats.text = "";
        
        string textToAdd = "";
        for (int i = 0; i < _dataGenerator.player.stats.Count; i++)
        {
            textToAdd +=
                $"{i + 1}\t\t{_dataGenerator.player.stats[i].x}\t\t{_dataGenerator.player.stats[i].y}\t\t{_dataGenerator.player.stats[i].z}\n";
        }
        playerStats.text = textToAdd;
    }

    private void FillEnemyStats()
    {
        enemyScrollView.SetActive(true);
        enemyStatsHeader.gameObject.SetActive(true);
        newEnemyDropdown.gameObject.SetActive(false);
        newEnemyGenButton.SetActive(false);
        regenerateEnemyButton.SetActive(true);
        CreateOutputCurve();
        
        enemyType.text = $" {_dataGenerator.entityClassesList[_index]} Enemy";
        enemyStats.text = "";
        
        string textToAdd = "";
        for (int i = 0; i < _dataGenerator.enemies[_index].stats.Count; i++)
        {
            textToAdd +=
                $"{i + 1}\t\t{_dataGenerator.enemies[_index].stats[i].x}\t\t{_dataGenerator.enemies[_index].stats[i].y}\t\t{_dataGenerator.enemies[_index].stats[i].z}\n";
        }
        enemyStats.text = textToAdd;
    }
    
    private void ShowEnemyCreator()
    {
        enemyStatsHeader.gameObject.SetActive(false);
        enemyScrollView.SetActive(false);
        enemyType.text = "New Enemy";
        rightButton.SetActive(false);
        
        newEnemyDropdown.gameObject.SetActive(true);
        newEnemyGenButton.SetActive(true);
        regenerateEnemyButton.SetActive(false);

        newEnemyDropdown.GetComponent<TMP_Dropdown>().options.Clear();

        List<EntityClass> possibleOptions = new List<EntityClass>
        {
            EntityClass.None,
            EntityClass.Tank,
            EntityClass.Warrior,
            EntityClass.Rogue,
            EntityClass.Mage
        };

        for (int i = 0; i < possibleOptions.Count; i++)
        {
            for (int j = 0; j < _dataGenerator.entityClassesList.Count; j++)
            {
                if (possibleOptions[i] == _dataGenerator.entityClassesList[j])
                {
                    possibleOptions.RemoveAt(i);
                }
            }
        }
        
        newEnemyDropdown.options.Clear();
        newEnemyDropdown.options = new List<EntityClass>(possibleOptions);
        for (int i = 0; i < possibleOptions.Count; i++)
        {
            newEnemyDropdown.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(possibleOptions[i].ToString()));
        }
        newEnemyDropdown.GetComponent<TMP_Dropdown>().value = 0;
    }

    private bool CheckForEnemy()
    {
        return (_dataGenerator.enemies.Count - 1) < _index;
    }

    private void FillChart()
    {
        if (CheckForEnemy())
        {
            ShowEnemyCreator();
        }
        else
        {
            FillEnemyStats();
        }
    }

    #endregion
}