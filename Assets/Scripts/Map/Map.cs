using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public string AssessTile(Vector2 pos)
    {
        string returnTile = "";
        bool[] grid = { false, false, false, false, false, false, false, false};
        byte index = 0;
        for(int i = (int)pos.x - 1; i <= (int)pos.x + 1; i++)
        {
            for (int j = (int)pos.y - 1; j <= (int)pos.y + 1; j++)
            {
                if (!(i == (int)pos.x && j == (int)pos.y))
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(i, j), Vector2.zero);
                    if(hitInfo.collider != null)
                    {
                        if (hitInfo.collider.GetComponent<IndependentBlock>() != null)
                        {
                            grid[index] = true;
                        }
                    }
                    
                    index++;
                }
                
            }
         }

        returnTile = ModifyShape(grid);
        return returnTile;
    }

    public void ChangeSludgeTile()
    {

    }

    public void ChangeTile(Vector2 pos, string to)
    {
        bool[] grid = { false, false, false, false, false, false, false, false };
        byte index = 0;
        for (int i = (int)pos.x - 1; i <= (int)pos.x + 1; i++)
        {
            for (int j = (int)pos.y - 1; j <= (int)pos.y + 1; j++)
            {
                if (!(i == (int)pos.x && j == (int)pos.y))
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(i, j), Vector2.zero);
                    if (hitInfo.collider != null)
                    {
                        if (hitInfo.collider.GetComponent<IndependentBlock>() != null)
                        {
                            grid[index] = true;
                        }
                    }

                    index++;
                }

            }
        }
    }

    private string ModifyShape(bool[] grid)
    {
        if (grid[0] && grid[1] && grid[2] && grid[3] && grid[4] && grid[5] && grid[6] && grid[7])
        {
            return "ALL-1234";
        } else if (grid[1] && grid[2] && grid[3] && grid[4] && grid[5] && grid[6] && grid[7])
        {
            return "ALL-123";
        } else if (grid[0] && grid[1] && grid[2] && grid[3] && grid[4] && grid[6] && grid[7])
        {
            return "ALL-124";
        } else if (grid[0] && grid[1] && grid[2] && grid[3] && grid[4] && grid[5] && grid[6])
        {
            return "ALL-134";
        } else if (grid[0] && grid[1] && grid[3] && grid[4] && grid[5] && grid[6] && grid[7])
        {
            return "ALL-234";
        } else if (grid[0] && grid[1] && grid[3] && grid[4] && grid[5] && grid[6])
        {
            return "ALL-34";
        } else if (grid[0] && grid[1] && grid[3] && grid[4] && grid[6] && grid[7])
        {
            return "ALL-24";
        } else if (grid[1] && grid[3] && grid[4] && grid[5] && grid[6] && grid[7])
        {
            return "ALL-23";
        } else if (grid[0] && grid[1] && grid[2] && grid[3] && grid[4] && grid[6])
        {
            return "ALL-14";
        } else if (grid[1] && grid[2] && grid[3] && grid[4] && grid[5] && grid[6])
        {
            return "ALL-13";
        } else if (grid[1] && grid[2] && grid[3] && grid[4] && grid[6] && grid[7])
        {
            return "ALL-12";
        } else if (grid[0] && grid[1] && grid[3] && grid[4] && grid[6])
        {
            return "ALL-4";
        } else if (grid[1] && grid[3] && grid[4] && grid[5] && grid[6])
        {
            return "ALL-3";
        } else if (grid[1] && grid[3] && grid[4] && grid[6] && grid[7])
        {
            return "ALL-2";
        } else if (grid[1] && grid[2] && grid[3] && grid[4] && grid[6])
        {
            return "ALL-1";
        } else if (grid[1] && grid[3] && grid[4] && grid[6])
        {
            return "ALL";
        } else if (grid[1] && grid[2] && grid[4] && grid[6] && grid[7])
        {
            return "N-3XD-12";
        } else if (grid[1] && grid[4] && grid[6] && grid[7])
        {
            return "N-3XD-2";
        } else if (grid[1] && grid[2] && grid[4] && grid[6])
        {
            return "N-3XD-1";
        } else if (grid[1] && grid[4] && grid[6])
        {
            return "N-3XD";
        } else if (grid[0] && grid[1] && grid[3] && grid[5] && grid[6])
        {
            return "N-3XU-34";
        } else if (grid[1] && grid[3] && grid[5] && grid[6])
        {
            return "N-3XU-3";
        } else if (grid[0] && grid[1] && grid[3] && grid[6])
        {
            return "N-3XU-4";
        } else if (grid[1] && grid[3] && grid[6])
        {
            return "N-3XU";
        } else if (grid[3] && grid[4] && grid[5] && grid[6] && grid[7])
        {
            return "N-3XL-23";
        } else if (grid[3] && grid[4] && grid[5] && grid[6])
        {
            return "N-3XL-3";
        } else if (grid[3] && grid[4] && grid[6] && grid[7])
        {
            return "N-3XL-2";
        } else if (grid[3] && grid[4] && grid[6])
        {
            return "N-3XL";
        } else if (grid[0] && grid[1] && grid[2] && grid[3] && grid[4])
        {
            return "N-3XR-14";
        } else if (grid[0] && grid[1] && grid[3] && grid[4])
        {
            return "N-3XR-4";
        } else if (grid[1] && grid[2] && grid[3] && grid[4])
        {
            return "N-3XR-1";
        } else if (grid[1] && grid[3] && grid[4])
        {
            return "N-3XR";
        } else if (grid[3] && grid[5] && grid[6])
        {
            return "N-DR-2";
        } else if (grid[3] && grid[6])
        {
            return "N-DR";
        }  else if (grid[0] && grid[1] && grid[3])
        {
            return "N-DL-2";
        } else if (grid[1] && grid[3])
        {
            return "N-DL";
        } else if (grid[4] && grid[6] && grid[7])
        {
            return "N-UR-2";
        } else if (grid[4] && grid[6])
        {
            return "N-UR";
        } else if (grid[1] && grid[2] && grid[4])
        {
            return "N-UL-2";
        } else if (grid[1] && grid[4])
        {
            return "N-UL";
        } else if (grid[1] && grid[6])
        {
            return "N-LR";
        } else if (grid[3] && grid[4])
        {
            return "N-UD";
        } else if (grid[1])
        {
            return "N-L";
        } else if (grid[3])
        {
            return "N-D";
        } else if (grid[6])
        {
            return "N-R";
        } else if (grid[4])
        {
            return "N-U";
        }
        return "N-N";
    }
}
