using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TR5TrainerTest.Logger;

namespace TR5TrainerTest
{
   public unsafe class Minimap
    {
        public static (Bitmap b, room_info r, List<(int x, int y, int r)> doors, string info) GetMinimap(int room/*, bool drawDoors = true*/, int mousex = -1, int mousey = -1)
        {
            var rptr = (room_info*)MainWindow.pmr.ReadUInt32((byte*) 0x875154) + room;
            var rm = MainWindow.pmr.ReadStruct<room_info>((byte*) rptr);
            var fd = MainWindow.pmr.ReadStructArray<FLOOR_INFO>((byte*)rm.floor, rm.x_size * rm.y_size);
            var fdaptr = (short*)MainWindow.pmr.ReadUInt32((byte*) 0x875168);
            var bmp = new Bitmap(rm.x_size * 14 + 1, rm.y_size * 14 + 1);
            var doors = new List<(int x, int y, int r)>();
            var propx = 0f;
            var propz = 0f;
            var binfo = "";
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.Black);

                for (var y = 0; y < rm.y_size; y++)
                {
                    var off = y * rm.x_size;

                    gfx.DrawLine(Pens.Black, 0, y * 14, bmp.Width - 1, y * 14);

                    for (var x = 0; x < rm.x_size; x++)
                    {
                        var clr = Color.FromArgb(0, 200, 200); // default floor
                        var data = fd[x + off];

                        var wall = false;
                        var door = -1;
                        var box = false;
                        var death = false;
                        var monkey = false;
                        var trigger = -1;
                        var climb = 0;

                        if (((data.fx_box_stopper >> 4) & 2047) != 2047)
                        {
                            box = true;
                        }

                        if (data.floor == -127 && data.ceiling == -127)
                        {
                            wall = true;
                        }

                        if (data.index != 0)
                        {
                            for (var i = 0;; i++)
                            {
                                var floordata = MainWindow.pmr.ReadInt16((byte*) (fdaptr + data.index + i));
                                var fct = floordata & 0x1F;
                                var sub = (floordata & 0x7C00) >> 8;

                                if (fct == 1)
                                {
                                    i++;
                                    door = MainWindow.pmr.ReadInt16((byte*) (fdaptr + data.index + i));
                                    doors.Add((x, y, door));
                                }
                                else if (fct == 4)
                                {
                                    trigger = sub;
                                    i++;
                                }
                                else if (fct == 5)
                                {
                                    death = true;
                                }
                                else if (fct == 6)
                                {
                                    climb = sub;
                                }
                                else if (fct == 19)
                                {
                                    monkey = true;
                                }
                                else if (new[] {2, 3, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18}.Contains(fct))
                                {
                                    i++;
                                }

                                if ((floordata & 0x8000) != 0) // end data
                                    break;
                            }
                        }

                        if (mousex == x && mousey == y)
                        {
                            binfo = "#" + room + " (" + x + ", " + y + ")";
                        }

                        if (box)
                        {
                            clr = Color.FromArgb(100, 100, 100);
                            if (mousex == x && mousey == y) binfo += ", BOX";
                        }
                        else if (trigger != -1)
                        {
                            clr = Color.FromArgb(200, 0, 200);
                            if (mousex == x && mousey == y) binfo += ", TRIGGER(" + trigger + ")";
                        }
                        else if (death)
                        {
                            clr = Color.FromArgb(0, 200, 0);
                            if (mousex == x && mousey == y) binfo += ", DEATH";
                        }
                        else if (monkey)
                        {
                            clr = Color.FromArgb(252, 96, 96);
                            if (mousex == x && mousey == y) binfo += ", MONKEY";
                        }
                        else if (door != -1)
                        {
                            clr = Color.FromArgb(0, 0, 0);
                            if (mousex == x && mousey == y) binfo += ", DOOR(" + door + ")";
                        }
                        else if (x == 0 || x == rm.x_size - 1 ||
                                 y == 0 || y == rm.y_size - 1)
                        {
                            clr = Color.FromArgb(152, 152, 152);
                        }
                        else if (wall)
                        {
                            clr = Color.FromArgb(0, 160, 0);
                            if (mousex == x && mousey == y) binfo += ", WALL";
                        }

                        gfx.FillRectangle(new SolidBrush(clr), x * 14 + 1, y * 14 + 1, 13, 13);

                        var climbBrs = new SolidBrush(Color.FromArgb(0, 80, 0));
                        if ((climb & 2) == 2) // +Z
                        {
                            gfx.FillRectangle(climbBrs, x * 14 + 1, y * 14 + 12, 13, 2);
                        }
                        if ((climb & 8) == 8) // -Z
                        {
                            gfx.FillRectangle(climbBrs, x * 14 + 1, y * 14 + 1, 13, 2);
                        }

                        if ((climb & 1) == 1) // +X
                        {
                            gfx.FillRectangle(climbBrs, x * 14 + 12, y * 14 + 1, 2, 13);
                        }
                        if ((climb & 4) == 4) // -X
                        {
                            gfx.FillRectangle(climbBrs, x * 14 + 1, y * 14 + 1, 2, 13);
                        }

                        
                    }
                }

                gfx.DrawLine(Pens.Black, 0, bmp.Height - 1, bmp.Width - 1, bmp.Height - 1);

                //if (drawDoors)
                //{
                    var lara = MainWindow.pmr.ReadStruct<lara_info>((byte*) 0xe5bd60);
                    var item = MainWindow.pmr.ReadStruct<ITEM_INFO>((byte*) ((ITEM_INFO*) MainWindow.pmr.ReadUInt32((byte*) 0xeeeff0) + lara.item_number));

                    var relz = item.pos.x_pos - rm.x;
                    var relx = item.pos.z_pos - rm.z;

                    propx = (float) Math.Round((relx * 14) / 1024.0);
                    propz = (float) Math.Round((relz * 14) / 1024.0);

                    // gfx.FillEllipse(Brushes.Red, propx - 2.5f, propz - 2.5f, 5, 5);



                    var angle = (ushort) (item.pos.y_rot) * Math.PI / 32768.0;
                    var arrowLength = 10;
                    var arrowWidth = 4;
                    gfx.FillPolygon(Brushes.Red, new[]
                    {
                        new PointF((float) (propx + arrowLength * Math.Cos(angle)), (float) (propz + arrowLength * Math.Sin(angle))),
                        new PointF((float) (propx + arrowWidth * Math.Cos(angle + Math.PI / 2)), (float) (propz + arrowWidth * Math.Sin(angle + Math.PI / 2))),
                        new PointF((float) (propx - arrowWidth * Math.Cos(angle + Math.PI / 2)), (float) (propz + arrowWidth * Math.Sin(angle - Math.PI / 2))),
                    });
                //}
                /* gfx.DrawLine(Pens.Red, propx, propz, 
                     (float)(propx + 7 * Math.Cos(angle)), (float)(propz + 7 * Math.Sin(angle)));*/
                //gfx.FillRectangle(Brushes.Red, propx - 1, propz - 1, 2, 2);
            }

           /* if (drawDoors && doors.Any())
            {
                var dis = doors.Select(x => (x, Math.Sqrt(Math.Pow(propx / 14 - x.x, 2) + Math.Pow(propz / 14 - x.y, 2))))
                    .OrderBy(x => x.Item2).First();
                if (dis.Item2 < 1.5)
                {
                    var (other, o_r, o_d) = GetMinimap(dis.Item1.r, false);
                    var o_w = o_r.x_size;
                    var o_h = o_r.y_size;
                    var o_x = 0;
                    var o_y = 0;
                    var t_x = 0;
                    var t_y = 0;
                    var wid = 0;
                    var hei = 0;

                    var cor_d = o_d.FirstOrDefault(x => x.x * 1024 + o_r.x == dis.Item1.x * 1024 + rm.x &&
                                                        x.y * 1024 + o_r.y == dis.Item1.y * 1024 + rm.y);

                    if (!cor_d.Equals(default(ValueTuple<int, int, int>)))
                    {



                        if (dis.Item1.x == 0)
                        {
                            t_x = o_w - 1;
                            o_y = dis.Item1.y;


                            var max_y = dis.Item1.y + o_h - 1;
                            wid = (rm.x_size + o_w - 1);
                            hei = Math.Max(max_y, rm.y_size);
                            o_y = dis.Item1.y - 1;
                           

                        }

                        var final = new Bitmap(wid * 14 + 1, hei * 14 + 1);

                        using (var gfx = Graphics.FromImage(final))
                        {
                            gfx.Clear(Color.Fuchsia);

                            gfx.DrawImageUnscaled(other, o_x * 14, o_y * 14);
                            gfx.DrawImageUnscaled(bmp, t_x * 14, t_y * 14);
                        }

                        bmp = final;

                    }
                }
            }*/

            return (bmp, rm, doors, binfo);
        }
    }
}
