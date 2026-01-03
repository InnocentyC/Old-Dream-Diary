using System.Collections;
using UnityEngine;
/*对话后停 0.5 秒
动画前后缓冲
镜头切换节奏控制
*/

    [CreateAssetMenu(menuName = "Game/State Actions/Wait")]
    public class WaitAction : StateAction
    {
        public float duration = 1f;

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(duration);
        }
    }
