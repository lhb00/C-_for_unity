
public abstract class State<T> where T : BaseGameEntity // class
{
    // 해당 상태를 시작할 때 1회 호출
    public abstract void Enter(T entity);

    // 해당 상태를 업데이트할 때 매 프레임 호출
    public abstract void Execute(T entity);

    // 해당 상태를 종료할 때 1회 호출
    public abstract void Exit(T entity);

    // 메시지를 받았을 때 1회 호출
    public abstract bool OnMessage(T entity, Telegram telegram);
}
