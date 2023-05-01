using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DialogueController : MonoBehaviour
{
    public static string playerName = "";

    public TMP_Text dialogueText;
    public TMP_Text playerTypingText;
    public TMP_Text instructionText;
    
    [FormerlySerializedAs("dialogue")] public List<Dialogue> story;
    public int curDialogue = 0;

    public float textSpeed = 80f; // 1337 hacker typing speed
    private int currentTypingCharacter;

    private void Start()
    {
        NextDialogue();

        print(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
        foreach (Dialogue dialogue in story)
        {
            dialogue.text = dialogue.text.Replace("%futureyear%", GetFutureYear())
                .Replace("%year%", GetRealYear());
        }
    }

    private void Update()
    {
        if (curDialogue >= story.Count) return;
        Dialogue dialogue = story[curDialogue];

        if (dialogue.character == Character.Player)
        {
            bool isDialogueOver = currentTypingCharacter >= dialogue.text.Length;
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (isDialogueOver)
                {
                    playerTypingText.gameObject.SetActive(false);
                    AddTextToDialogue($"%date% > <color=#ffffff>{playerName}</color> > {dialogue.text}");
                    curDialogue++;
                    NextDialogue();
                }
            }
            else if (!isDialogueOver && Input.anyKeyDown &&
                     !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            {
                playerTypingText.text += dialogue.text[currentTypingCharacter];
                currentTypingCharacter++;
                if (currentTypingCharacter < dialogue.text.Length)
                {
                    playerTypingText.text += dialogue.text[currentTypingCharacter];
                    currentTypingCharacter++;
                }
                if (currentTypingCharacter >= dialogue.text.Length)
                {
                    instructionText.text = "(press Enter)";
                }
            }
        } else if (dialogue.character == Character.PlayerName)
        {
            if (Regex.IsMatch(Input.inputString, @"^[a-zA-Z0-9_]+$"))
            {
                playerName += Input.inputString;
            } else if (Input.GetKeyDown(KeyCode.Backspace) && playerName.Length > 0)
            {
                playerName = playerName.Remove(playerName.Length - 1);
            } else if (Input.GetKeyDown(KeyCode.Return) && playerName.Length > 0)
            {
                playerTypingText.gameObject.SetActive(false);
                curDialogue++;
                NextDialogue();
            }
            playerTypingText.text = "> " + playerName;
        }
    }

    public string GetFutureDate()
    {
        return DateTime.Now.AddYears(1).ToString("MM/dd/yyyy HH:mm:ss");
    }

    public string GetFutureYear()
    {
        return DateTime.Now.AddYears(1).ToString("yyyy");
    }
    public string GetRealYear()
    {
        return DateTime.Now.ToString("yyyy");
    }

    public void NextDialogue()
    {
        if (curDialogue >= story.Count) return;

        Dialogue dialogue = story[curDialogue];

        if (dialogue.character == Character.Player || dialogue.character == Character.PlayerName)
        {
            SetupPlayerText(dialogue);
            return;
        }
        else
        {
            playerTypingText.gameObject.SetActive(false);
        }
        
        float timeForDialogue;
        if (dialogue.customTime > 0) timeForDialogue = dialogue.customTime;
        else
            switch (dialogue.character)
            {
                case Character.Npc:
                    timeForDialogue = dialogue.text.Length / textSpeed + 1;
                    break;
                case Character.System:
                    timeForDialogue = 0.1f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        timeForDialogue += Random.Range(0, 0.2f);
        
        Timer.Register(timeForDialogue, () =>
        {
            switch (dialogue.character)
            {
                case Character.Player:
                case Character.Npc:
                    string[] split = dialogue.text.Split('>', 2);
                    string name = split[0];
                    string message = split[1];
                    if (name == "palette ") AddTextToDialogue($"%date% > <color=#9090ff>{name}</color>>{message}");
                    else AddTextToDialogue($"%date% > {name}>{message}");

                    break;
                case Character.System:
                    AddTextToDialogue($"SYS > {dialogue.text}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            curDialogue++;
            NextDialogue();
        });
    }

    public void AddTextToDialogue(string text)
    {
        dialogueText.text += "\n" + text
            .Replace("%player%", playerName)
            .Replace("%date%", $"<color=#006000>{GetFutureDate()}</color>");
    }

    public void SetupPlayerText(Dialogue dialogue)
    {
        playerTypingText.text = "> ";
        playerTypingText.gameObject.SetActive(true);
        if (dialogue.character == Character.PlayerName)
            instructionText.text = "(enter your hacker alias, press Enter when done)";
        else if (dialogue.text.Length == 0) instructionText.text = "(press Enter to begin)";
        else instructionText.text = "(start typing)";
        currentTypingCharacter = 0;
    }

    private static Color GetRandomColor(string text)
    {
        Random.InitState(text.GetHashCode());
        Random.Range(0, 1);
        Color randomColor = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        return randomColor;
    }

    [Serializable]
    public class Dialogue
    {
        [TextArea(2,4)] public string text;
        public Character character = Character.Npc;
        public float customTime;
    }

    public enum Character
    {
        Npc,
        System,
        Player,
        PlayerName,
    }
}