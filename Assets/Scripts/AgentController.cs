using UnityEngine;


public class AgentController : MonoBehaviour
{
    public TilemapPathfinder agent;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
            target.z = agent.transform.position.z;
            agent.Move(target);
        }
    }
}
