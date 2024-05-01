using System.Collections;
using System.Collections.Generic;
using BTree;
using UnityEngine;
// ReSharper disable CommentTypo

public class MoveToPlayer : Leaf<ITreeContext>
{
    private Enemy  _enemy;
    private Vector3 _target;
    
    protected override void OnSetup()
    {
        _enemy = Agent as Enemy;
    }

    protected override void OnEnter()
    {
        if (_enemy.CheckIsIdle())
        {
            _enemy.RotateOnMove = true;
            var pos = GetPlayerPos();
            _target = _enemy.MoveTo(pos);
            _enemy.MovingToPlayer = true;
        }
        else
        {
            Response.Result = Result.Failure;
        }
    }

    protected override void OnExecute()
    {
        //Проверка наличия ссылки на игрока у игровой сущности
        if (_enemy.Player is null)
        {
            Response.Result = Result.Failure;//Установка неудачного результата
            return;
        }
        //Обновление точки назначения если игрок отошел от нее на определенное расстояние
        if ((_target - _enemy.Player.Position).magnitude > 0.25f)
        {
            var pos = _enemy.Player.Position;//Получение координат игрока
            _target = _enemy.MoveTo(pos);//Установка новой точки назначения
        }
        //Проверка расстояния между игроком и игровой сущностью
        if ((_enemy.transform.position - _enemy.Player.Position).magnitude <= 2)
        {
            Response.Result = Result.Success;//Установка успешного результата
        }
    }

    protected override void OnExit()
    {
        _enemy.MovingToPlayer = false;
    }

    protected override void OnReset()
    {
        _target = default;
    }

    protected override void OnFail()
    {
        
    }

    private Vector3 GetPlayerPos()
    {
        return _enemy.Player.Position;
    }
}
