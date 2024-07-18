using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Block> L_BlockInLevel;

    public List<GameObject> L_SkinBlock;

    public float MinScale = 1f;

    public void CreateBlock(int id, E_TypeSpawnBlock TypeSpawn, float rand)
    {
        EventManager.EmitEvent(EventContains.UPDATE_SCORE);

        switch (TypeSpawn)
        {
            case E_TypeSpawnBlock.Left:
                int randBlock = Random.Range(0, L_SkinBlock.Count);

                float randScale = Random.Range(MinScale, 1);

                GameObject Block = Instantiate(L_SkinBlock[randBlock].gameObject, new Vector3(L_BlockInLevel[L_BlockInLevel.Count - 1].transform.position.x, L_BlockInLevel[L_BlockInLevel.Count - 1].transform.position.y + 1f, L_BlockInLevel[L_BlockInLevel.Count - 1].transform.position.z + rand), Quaternion.identity);
                Block.transform.SetParent(transform);

                Block BlockLeft;


                if (Block.transform.GetChild(0).GetComponent<Block>())
                {
                    BlockLeft = Block.transform.GetChild(0).GetComponent<Block>();
                }
                else
                {
                    BlockLeft = Block.transform.GetComponent<Block>();
                }




                BlockLeft.id = id;
                BlockLeft.transform.position = new Vector3(BlockLeft.transform.position.x, 1f, BlockLeft.transform.position.z);
                BlockLeft.transform.localScale = new Vector3(randScale, randScale, randScale);
                BlockLeft.MoveToTargetPoint();

                L_BlockInLevel.Add(BlockLeft);

                break;
            case E_TypeSpawnBlock.Forward:
                int randBlockForward = Random.Range(0, L_SkinBlock.Count);

                float randScaleForward = Random.Range(MinScale, 1);
                GameObject Block1 = Instantiate(L_SkinBlock[randBlockForward].gameObject, new Vector3(L_BlockInLevel[L_BlockInLevel.Count - 1].transform.position.x + rand, L_BlockInLevel[L_BlockInLevel.Count - 1].transform.position.y + 1f, L_BlockInLevel[L_BlockInLevel.Count - 1].transform.position.z), Quaternion.identity);

                Block1.transform.SetParent(transform);

                Block blockTop;

                if (Block1.transform.GetChild(0).GetComponent<Block>())
                {
                    blockTop = Block1.transform.GetChild(0).GetComponent<Block>();
                }
                else
                {
                    blockTop = Block1.transform.GetComponent<Block>();
                }

                blockTop.id = id;
                blockTop.transform.position = new Vector3(blockTop.transform.position.x, 1f, blockTop.transform.position.z);
                blockTop.transform.localScale = new Vector3(randScaleForward, randScaleForward, randScaleForward);
                blockTop.MoveToTargetPoint();

                L_BlockInLevel.Add(blockTop);

                break;
            default:
                break;
        }
    }
}
