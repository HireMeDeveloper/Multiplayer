using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Window : Damageable
{
    [SerializeField] private List<Board> boards = new List<Board>();
    [SerializeField] private int healthPerBoard = 2;
    private Stack<Board> boardStack = new Stack<Board>();
    private Stack<Board> brokenBoardStack = new Stack<Board>();
    private int currentBoardHealth;

    public bool isBroken { get; private set; } = false;

    public UnityEvent onBreak;
    public UnityEvent onRepair;

    private void Start()
    {
        ResetBoards();
    }
    public void ResetBoards()
    {
        isBroken = false;
        foreach (var board in boards)
        {
            boardStack.Push(board);
            board.RepairBoard();
        }
        brokenBoardStack.Clear();
        ResetBoardHealth();
    }
    public override void damage(int damage, Player damager)
    {
        base.damage(damage, damager);
        DamageBoard();
        Debug.Log("Hit by zombies");
    }

    public override void death()
    {
        base.death();

    }

    private void ResetBoardHealth()
    {
        currentBoardHealth = healthPerBoard;
    }

    private void DamageBoard()
    {
        if (boardStack.Count == 0) return;
        if (currentBoardHealth == 0)
        {
            BreakBoard();
            return;
        }
        currentBoardHealth--;
        boardStack.Peek().DamageBoard();
    }

    private void BreakBoard()
    {
        if (boardStack.Count == 0) return;
        var board = boardStack.Pop();
        board.BreakBoard();
        brokenBoardStack.Push(board);
        ResetBoardHealth();

        if(boardStack.Count == 0)
        {
            Debug.Log("Window machine broke");
            onBreak.Invoke();
            isBroken = true;
        }
    }

    public bool RepairBoard()
    {
        if (brokenBoardStack.Count == 0) return false;

        var board = brokenBoardStack.Pop();
        board.RepairBoard();
        boardStack.Push(board);
        ResetBoardHealth();

        onRepair.Invoke();
        isBroken = false;

        return true;
    }

}
