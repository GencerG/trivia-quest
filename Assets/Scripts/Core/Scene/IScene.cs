using System.Collections;

public interface IScene
{
    IEnumerator Initialize();
    void Destroy();
}
