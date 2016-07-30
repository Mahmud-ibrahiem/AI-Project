﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using System.Collections;




// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AI_project
{
    /// <summary>
    // This project is to simulate an AI solution of a puzzle
    /// </summary>

    public sealed partial class MainPage : Page
    {
        //int NumberOfAvailableMoves = 0;
        static public int[] desired = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        public string[] moves = new string[4] {"null","null","null","null" };
        public class Node

        {
            public int NumberOfAvailableMoves { get; set; } // this has the number of moves that could be done from this node
            public bool explored;  // returns a boolean value whether the node has been explored or not
            public double heuristic { get; set; } // has the heuristic value of the node
            public double AstarFunc { get; set; } // has the value of the Astar function of the node

            public double parentid { get; set; }  // the id of the parent node
            public string prevmove { get; set; }  // this shows the name of the previous move left or right
            public int cost { get; set; } // The cost of the node
            public double nodeid { get; set; }  // the node id which is an identifier for the node
            public int[] state = new int[8];   // the first member of the array has the position of the pieceone button
            public int[] PositionsTomoveTo = new int[4];  //the matrix has an array of the positions to move to
            public Node() { AstarFunc = 0; cost = 0; nodeid = 0; parentid = 0; PositionsTomoveTo = null; NumberOfAvailableMoves = 0; heuristic = 100; /*inf*/ explored = false; }  // constructor of the class
            public Node(Node m) { this.nodeid = m.nodeid; this.parentid = m.parentid; this.state = m.state; PositionsTomoveTo = null; AstarFunc = 0; NumberOfAvailableMoves = 0; cost = 0;prevmove = "null"; heuristic = 100; /*inf*/ explored = false; }  // constructor with an arguement
            public Node getCopy()
            {
                Node temp = new Node();
                temp.parentid = nodeid;
                temp.nodeid = nodeid;
                temp.state = state;
                return temp;
            }

        }
        int clicked = 0; Node parentnode = new Node();
        public Node produce(Node m)
        {
            Node temp = m;
            temp.parentid = m.parentid;
            temp.nodeid = m.nodeid;
            temp.state = m.state;
            return temp;
        } // produce a node with given arguments
        public void getpositions(ref Node gpnode)
        {
            //  int[] positions = new int[8];

            gpnode.state[0] = Grid.GetRow(pieceOne) * 3 + Grid.GetColumn(pieceOne) + 1;
            gpnode.state[1] = Grid.GetRow(piecetwo) * 3 + Grid.GetColumn(piecetwo) + 1;
            gpnode.state[2] = Grid.GetRow(piecethree) * 3 + Grid.GetColumn(piecethree) + 1;
            gpnode.state[3] = Grid.GetRow(piecefour) * 3 + Grid.GetColumn(piecefour) + 1;
            gpnode.state[4] = Grid.GetRow(piecefive) * 3 + Grid.GetColumn(piecefive) + 1;
            gpnode.state[5] = Grid.GetRow(piecesix) * 3 + Grid.GetColumn(piecesix) + 1;
            gpnode.state[6] = Grid.GetRow(pieceSeven) * 3 + Grid.GetColumn(pieceSeven) + 1;
            gpnode.state[7] = Grid.GetRow(pieceeight) * 3 + Grid.GetColumn(pieceeight) + 1;
        }  // gets the position of the current figure
        public void copyobj(ref Node cpynode, int[] state, double parentid, int cost, string setprevmove)
        {
            //  int[] positions = new int[8];
          
            cpynode.state[0] = state[0];
            cpynode.state[1] = state[1];
            cpynode.state[2] = state[2];
            cpynode.state[3] = state[3];
            cpynode.state[4] = state[4];
            cpynode.state[5] = state[5];
            cpynode.state[6] = state[6];
            cpynode.state[7] = state[7];
            cpynode.parentid = parentid;
            cpynode.cost = cost + 1;
            cpynode.prevmove=setprevmove;
            
        }  // makes a copy of a node

        public void calcheuristic(ref Node cpynode)  // calculates the heuristic of a node
        {
            int realplace = 0;
            int count = 0;
            double currentrow = 0;
            double supposedrow = 0;
            int currentcolumn = 0;
            int supposedcolumn = 0;
            double heuristic = 0;

            for (count = 0; count < cpynode.state.Length; count++)
            {
                realplace = count + 1;
                currentcolumn = cpynode.state[count] % 3;
                if (currentcolumn == 0) { currentcolumn = 3; }
                supposedcolumn = realplace % 3;
                if (supposedcolumn == 0) { supposedcolumn = 3; }
                currentrow = Math.Ceiling(cpynode.state[count] / 3.0);
                supposedrow = Math.Ceiling(realplace / 3.0);
                heuristic += Math.Abs(currentrow - supposedrow) + Math.Abs(currentcolumn - supposedcolumn);
                //heuristic += Math.Abs(Math.Ceiling(Convert.ToDouble(realplace) / 3.0)- Math.Ceiling(Convert.ToDouble(cpynode.state[count]) / 3.0))+ Math.Abs((realplace) % 3 - (cpynode.state[count] %3));
            }
            cpynode.heuristic = heuristic;

        }    // calculates the heuristics
        public void copyobj(ref Node cpynode, ref Node sourcenode)
        {
            cpynode.state[0] = sourcenode.state[0];
            cpynode.state[1] = sourcenode.state[1];
            cpynode.state[2] = sourcenode.state[2];
            cpynode.state[3] = sourcenode.state[3];
            cpynode.state[4] = sourcenode.state[4];
            cpynode.state[5] = sourcenode.state[5];
            cpynode.state[6] = sourcenode.state[6];
            cpynode.state[7] = sourcenode.state[7];
            cpynode.parentid = sourcenode.parentid;
            cpynode.nodeid = sourcenode.nodeid;
            cpynode.cost = sourcenode.cost;
            cpynode.prevmove = sourcenode.prevmove;
            cpynode.heuristic = sourcenode.heuristic;
        }  // copies an exact copy of the object with no chan
        public void Setlocations(int[] locations)
        {
            int i;
            FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            parentnode.state[0] = locations[0];
            parentnode.state[1] = locations[1];
            parentnode.state[2] = locations[2];
            parentnode.state[3] = locations[3];
            parentnode.state[4] = locations[4];
            parentnode.state[5] = locations[5];
            parentnode.state[6] = locations[6];
            parentnode.state[7] = locations[7];
            for (i = 0; i < 8; i++)
            {
                double q;

                q = (double)parentnode.state[i];
                q = (q - 1) / 3;
                q = Math.Floor(q);
                Grid.SetRow(buttons[i], (int)q);
                Grid.SetColumn(buttons[i], (parentnode.state[i] - 1) % 3);
            }

        }  // sets the buttons to a specific location
        public bool goal(Node checkgoal)
        {
            int nodesdistreach = 0;

            for (int count = 0; count < 8; count++)
            {
                if (checkgoal.state[count] == desired[count])
                {
                    nodesdistreach++;
                }
            }
            if (nodesdistreach == 8)
            {

                //await dialog.ShowAsync();
                return true;
            }
            else
                return false;
        }  // checks whether the goal has been acheived or not
        
        public int blankposition(ref Node bpNode)
        {
            int counter;
            int blank = new int();
            for (int i = 1; i < 10; i++)
            {
                counter = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (bpNode.state[j] != i)
                    {
                        counter++;

                    }
                }
                if (counter == 8) { blank = i; }
            }
            return blank;
        } // get the blank position

        public bool solveable(int[] receivesee)
        {
            int defineconst = 0;
            for (int i = 0; i < receivesee.Length; i++)
            {
                for (int j = i + 1; j < receivesee.Length; j++)
                {
                    if (receivesee[i] > receivesee[j] && receivesee[j] != 0)
                    {
                        defineconst++;
                    }

                }

            }
            if (defineconst % 2 == 1) { return false; }
            else
                return true;
        }  // checks the solveability of a puzzle

        public bool EqualMatrix(int[] vectorone, int[] vectortwo)
        {
            int correlation = 0;
            
                for (int j = 0; j < vectorone.Length; j++)
                {
                    if (vectorone[j]== vectortwo[j])
                    {
                        correlation++;
                    }

                }

            
            if (correlation ==8) { return true; }
            else
                return false;
        }  // checks if two matrices are equal or not

        public int[] realposition(ref Node node)
        {
            int[] see = new int[9];
            for (int i = 0; i < node.state.Length; i++)
            {
                see[node.state[i] - 1] = i + 1;
            }

            return see;
        }  // gets the real position viewwise of the puzzle
        public int[] Action(ref Node Acnode)
        {
            Acnode.NumberOfAvailableMoves = 0;
            int[] Available = new int[4] { 0, 0, 0, 0 };
            /* if (blank == 1) {
                counter[0] = 2;
                counter[1] = 4;
                 }*/
            switch (blankposition(ref Acnode))
            {
                case 1:
                    Available[1] = 2;
                    moves[1] = "right";
                    Available[0] = 4;
                    moves[0] = "down";
                    break;
                case 2:
                    Available[2] = 1;
                    moves[2] = "left";
                    Available[1] = 3;
                    moves[1] = "right";
                    Available[0] = 5;
                    moves[0] = "down";
                    break;
                case 3:
                    Available[1] = 2;
                    moves[1] = "left";
                    Available[0] = 6;
                    moves[0] = "down";
                    break;
                case 4:
                    Available[0] = 1;
                    moves[0] = "up";
                    Available[2] = 5;
                    moves[2] = "right";
                    Available[1] = 7;
                    moves[1] = "down";
                    break;
                case 5:
                    Available[0] = 2;
                    moves[0] = "up";
                    Available[3] = 4;
                    moves[3] = "left";
                    Available[2] = 6;
                    moves[2] = "right";
                    Available[1] = 8;
                    moves[1] = "down";
                    break;
                case 6:
                    Available[0] = 3;
                    moves[0] = "up";
                    Available[1] = 5;
                    moves[1] = "left";
                    Available[2] = 9;
                    moves[2] = "down";
                    break;
                case 7:
                    Available[0] = 4;
                    moves[0] = "up";
                    Available[1] = 8;
                    moves[1] = "right";
                    break;
                case 8:
                    Available[0] = 5;
                    moves[0] = "up";
                    Available[2] = 7;
                    moves[2] = "left";
                    Available[1] = 9;
                    moves[1] = "right";
                    break;
                case 9:
                    Available[0] = 6;
                    moves[0] = "up";
                    Available[1] = 8;
                    moves[1] = "left";
                    break;
                default:

                    break;
            }
            for (int x = 0; x < Available.Length; x++)
            {
                if (Available[x] != 0)
                {
                    Acnode.NumberOfAvailableMoves += 1;
                }
            }
            return Available;
        } // get available position


        public void setnodeid(ref Node nodeset)
        {

            for (int C = 0; C < 8; C++)
            {
                if (C == 0) { nodeset.nodeid = 0; }
                nodeset.nodeid += nodeset.state[C] * Math.Pow(10, 7 - C); ;

            } // initiate desired locatiion in class
        }  // sets the id of a given node
        public MainPage()
        {
            this.InitializeComponent();
        }


        private async void Astar_Click(object sender, RoutedEventArgs e)
        {
            int C, j; int superposition = 0; FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            var Allnodes = new Dictionary<double, Node>();

            Queue<Node> frontier = new Queue<Node>();
            getpositions(ref parentnode);
            setnodeid(ref parentnode);
            int[] receivesee = new int[8];
            int explored = 0;
            receivesee = realposition(ref parentnode);

           



            if (!solveable(realposition(ref parentnode)))
            {
                var dialog = new Windows.UI.Popups.MessageDialog("this puzzle is not solveable", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            for (C = 0; C < 8; C++)
            {
                for (j = C + 1; j < 8; j++)
                {
                    if (parentnode.state[C] == parentnode.state[j])
                    {
                        superposition++;
                    }
                }
            }  // check for superpositions
            if (superposition > 0)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("there's a super-position error in your arrangement please fix it and then try to solve", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            parentnode.PositionsTomoveTo = Action(ref parentnode);
            Node[] cachenode = new Node[parentnode.NumberOfAvailableMoves];
            Node temp = new Node();
            Node temp2 = new Node();
            int frontiercount = 0;
            Allnodes.Add(parentnode.nodeid, parentnode);
            if (goal(parentnode))
            {

                var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            if (superposition == 0 && !goal(parentnode))
            {
                explored = explored + 1;
                copyobj(ref temp2, ref parentnode);
                double minAstarFunc = 100;
                while (!goal(temp2) && solveable(realposition(ref temp2)))
                {
                    for (j = 0; j < temp2.NumberOfAvailableMoves; j++)
                    {
                        cachenode[j] = new Node();
                        copyobj(ref cachenode[j], temp2.state, temp2.nodeid, temp2.cost, moves[j]);
                        int mohm = Array.IndexOf(cachenode[j].state, temp2.PositionsTomoveTo[j]);
                        cachenode[j].state[mohm] = blankposition(ref temp2);
                        setnodeid(ref cachenode[j]);
                        calcheuristic(ref cachenode[j]);
                        cachenode[j].AstarFunc = cachenode[j].cost + cachenode[j].heuristic;
                        if (!Allnodes.ContainsKey(cachenode[j].nodeid))
                        {
                            Allnodes.Add(cachenode[j].nodeid, cachenode[j]);
                            frontier.Enqueue(cachenode[j]);
                        }
                    }
                    frontiercount = frontier.Count;
                    for (j = 0; j < frontiercount; j++)
                    {
                        temp = frontier.Dequeue();
                        if (temp.AstarFunc <= minAstarFunc && temp.explored == false)
                        {
                            temp.explored = true;
                            copyobj(ref temp2, ref temp);
                            minAstarFunc = temp.heuristic;

                        }
                        frontier.Enqueue(temp);
                    }
                    if (explored - Allnodes.Count > 3) { minAstarFunc++; explored -= 4; }
                    explored++;
                    temp2.explored = true;
                    temp2.PositionsTomoveTo = Action(ref temp2);
                    cachenode = new Node[temp2.NumberOfAvailableMoves];
                    if (explored > 4000)
                    {
                        var dialog3 = new Windows.UI.Popups.MessageDialog("Over 4000 nodes have been explored in this search and total expanded nodes: " + Allnodes.Count + " , please try another kind of search !");
                        dialog3.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                        await dialog3.ShowAsync();
                        break;
                    }
                }
                int legalpath;
                if (goal(temp2) && solveable(realposition(ref temp2)))
                {


                    Stack<Node> pathstack = new Stack<Node>();
                    Allnodes[parentnode.nodeid].parentid = 0;

                    int path = 0;
                    string state;
                    int[] shownstate;
                    while (temp2.parentid != 0)
                    {
                        path++;
                        shownstate=realposition(ref temp2);
                        state = shownstate[0].ToString() + shownstate[1].ToString() + shownstate[2].ToString() + shownstate[3].ToString() + shownstate[4].ToString() + shownstate[5].ToString() + shownstate[6].ToString() + shownstate[7].ToString()+ shownstate[8].ToString()  ;
                        listview2.Items.Add(state);
                        listview1.Items.Add(path.ToString());
                        listview3.Items.Add(temp2.AstarFunc.ToString());
                        pathstack.Push(temp2);
                        temp2 = Allnodes[temp2.parentid];
                    }

                    legalpath = pathstack.Count;
                    var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached after exploring " + explored + " pathcost of " + legalpath + " and expanding " + frontier.Count + " nodes A* search");
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                    await dialog.ShowAsync();
                    MediaElement mysong = new MediaElement();
                    Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Audio");
                    Windows.Storage.StorageFile file = await folder.GetFileAsync("Tarara.mp3");
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    mysong.SetSource(stream, file.ContentType);
                    mysong.Play();
                    for (j = 0; j < legalpath; j++)
                    {
                        Setlocations(pathstack.Pop().state);
                        await Task.Delay(1300);
                    }
                }
            }
        }  // Applies the Astar algorithm

        private async void GBFS_Click(object sender, RoutedEventArgs e)
        {
            int C, j; int superposition = 0; FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            var Allnodes = new Dictionary<double, Node>();

            Queue<Node> frontier = new Queue<Node>();
            getpositions(ref parentnode);
            setnodeid(ref parentnode);
            int[] receivesee = new int[8];
            int explored = 0;
            receivesee = realposition(ref parentnode);

            if (!solveable(realposition(ref parentnode)))
            {
                var dialog = new Windows.UI.Popups.MessageDialog("this puzzle is not solveable", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            for (C = 0; C < 8; C++)
            {
                for (j = C + 1; j < 8; j++)
                {
                    if (parentnode.state[C] == parentnode.state[j])
                    {
                        superposition++;
                    }
                }
            }  // check for superpositions
            if (superposition > 0)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("there's a super-position error in your arrangement please fix it and then try to solve", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            parentnode.PositionsTomoveTo = Action(ref parentnode);
            Node[] cachenode = new Node[parentnode.NumberOfAvailableMoves];
            Node temp = new Node();
            Node temp2 = new Node();
            int frontiercount = 0;
            Allnodes.Add(parentnode.nodeid, parentnode);
            if (goal(parentnode))
            {

                var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }
            
            if (superposition == 0 && !goal(parentnode))
            {
                explored = explored + 1;
                copyobj(ref temp2, ref parentnode);
                double minheuristic = 100;
                while (!goal(temp2) && solveable(realposition(ref temp2)))
                {
                    //minheuristic = 100;
                    for (j = 0; j < temp2.NumberOfAvailableMoves; j++)
                    {
                        cachenode[j] = new Node();
                        copyobj(ref cachenode[j], temp2.state, temp2.nodeid, temp2.cost, moves[j]);
                        int mohm = Array.IndexOf(cachenode[j].state, temp2.PositionsTomoveTo[j]);
                        cachenode[j].state[mohm] = blankposition(ref temp2);
                        setnodeid(ref cachenode[j]);
                        calcheuristic(ref cachenode[j]);
                        if (!Allnodes.ContainsKey(cachenode[j].nodeid))
                        {
                            Allnodes.Add(cachenode[j].nodeid, cachenode[j]);
                            frontier.Enqueue(cachenode[j]);
                        }
                    }
                    frontiercount = frontier.Count;
                    for (j = 0; j < frontiercount; j++)
                    {
                        temp = frontier.Dequeue();
                        if (temp.heuristic <= minheuristic && temp.explored==false)
                        {
                            temp.explored = true;
                            copyobj(ref temp2, ref temp);
                            minheuristic = temp.heuristic;

                        }
                        frontier.Enqueue(temp);
                    }
                    if (explored - Allnodes.Count > 3) { minheuristic++;explored -= 4; }
                    explored++;
                   temp2.explored = true;
                    temp2.PositionsTomoveTo = Action(ref temp2);
                    cachenode = new Node[temp2.NumberOfAvailableMoves];
                    if (explored > 4000)
                    {
                        var dialog3 = new Windows.UI.Popups.MessageDialog("Over 4000 nodes have been explored in this search and total expanded nodes: " + Allnodes.Count + " , please try another kind of search !");
                        dialog3.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                        await dialog3.ShowAsync();
                        break;
                    }
                }
                int legalpath;
                if (goal(temp2) && solveable(realposition(ref temp2)))
                {


                    Stack<Node> pathstack = new Stack<Node>();


                    // temp = Allnodes[12345679];

                    Allnodes[parentnode.nodeid].parentid = 0;


                    int path = 0;
                    string state;
                    int[] shownstate;
                    while (temp2.parentid != 0)
                    {
                        path++;
                        shownstate = realposition(ref temp2);
                        state = shownstate[0].ToString() + shownstate[1].ToString() + shownstate[2].ToString() + shownstate[3].ToString() + shownstate[4].ToString() + shownstate[5].ToString() + shownstate[6].ToString() + shownstate[7].ToString() + shownstate[8].ToString();
                        listview2.Items.Add(state);
                        listview1.Items.Add(path.ToString());
                        listview3.Items.Add(temp2.heuristic.ToString());
                        pathstack.Push(temp2);
                        temp2 = Allnodes[temp2.parentid];
                    }

                    legalpath = pathstack.Count;
                    var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached after exploring " + explored + " pathcost of " + legalpath + " and expanding " + frontier.Count + " nodes GBFS search");
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                    await dialog.ShowAsync();
                    MediaElement mysong = new MediaElement();
                    Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Audio");
                    Windows.Storage.StorageFile file = await folder.GetFileAsync("Tarara.mp3");
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    mysong.SetSource(stream, file.ContentType);
                    mysong.Play();
                    for (j = 0; j < legalpath; j++)
                    {
                        Setlocations(pathstack.Pop().state);
                        await Task.Delay(1300);
                    }
                }
            }
        }  // Applies the GBFS algorithm
        private async void DFS_Click(object sender, RoutedEventArgs e)
        {
            int C, j; int superposition = 0; FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            var Allnodes = new Dictionary<double, Node>();

            Queue<Node> frontier = new Queue<Node>();
            Queue<Node> frontierup = new Queue<Node>(); // right
            Queue<Node> frontierdown = new Queue<Node>(); // left
            Queue<Node> frontierright = new Queue<Node>(); // up
            Queue<Node> frontierleft= new Queue<Node>(); // down

            getpositions(ref parentnode);
            setnodeid(ref parentnode);
            int[] receivesee = new int[8];
            int explored = 0;
       
            receivesee = realposition(ref parentnode);

            if (!solveable(realposition(ref parentnode)))
            {
                var dialog = new Windows.UI.Popups.MessageDialog("this puzzle is not solveable", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            for (C = 0; C < 8; C++)
            {
                for (j = C + 1; j < 8; j++)
                {
                    if (parentnode.state[C] == parentnode.state[j])
                    {
                        superposition++;
                    }
                }
            }  // check for superpositions
            if (superposition > 0)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("there's a super-position error in your arrangement please fix it and then try to solve", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            parentnode.PositionsTomoveTo = Action(ref parentnode);
            Node[] cachenode = new Node[parentnode.NumberOfAvailableMoves];
            Node temp = new Node();
            Node temp2 = new Node();

            Allnodes.Add(parentnode.nodeid, parentnode);
            if (goal(parentnode))
            {

                var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            if (superposition == 0 && !goal(parentnode))
            {
                // temp = parentnode;
                explored = explored + 1;
                temp2 = parentnode;
                while (!goal(temp2) && solveable(realposition(ref temp2)))
                {
                    for (j = 0; j < temp2.NumberOfAvailableMoves; j++)
                    {
                        cachenode[j] = new Node();
                        copyobj(ref cachenode[j], temp2.state, temp2.nodeid, temp2.cost,moves[j]);
                        int mohm = Array.IndexOf(cachenode[j].state, temp2.PositionsTomoveTo[j]);
                        cachenode[j].state[mohm] = blankposition(ref temp2);
                        setnodeid(ref cachenode[j]);


                        if (!Allnodes.ContainsKey(cachenode[j].nodeid))
                        {
                            Allnodes.Add(cachenode[j].nodeid, cachenode[j]);
                            switch (cachenode[j].prevmove)
                            {
                                case "up":
                                    frontierup.Enqueue(cachenode[j]);
                                    break;
                                case "down":
                                    frontierdown.Enqueue(cachenode[j]);
                                    break;
                                case "right":
                                    frontierright.Enqueue(cachenode[j]);
                                    break;
                                case "left":
                                    frontierleft.Enqueue(cachenode[j]);
                                    break;
                                default:

                                    break;
                            }

                        }
                     
                    }

                    if (frontierup.Count > 0)
                    {
                        temp2 = frontierup.Dequeue();
                        temp2.PositionsTomoveTo = Action(ref temp2);
                    }
                    else
                    {
                        if (frontierdown.Count > 0)
                        {
                            temp2 = frontierdown.Dequeue();
                            temp2.PositionsTomoveTo = Action(ref temp2);
                        }
                        else
                        {
                            if (frontierright.Count > 0)
                            {
                                temp2 = frontierright.Dequeue();
                                temp2.PositionsTomoveTo = Action(ref temp2);
                            }
                            else
                            {
                                temp2 = frontierleft.Dequeue();
                                temp2.PositionsTomoveTo = Action(ref temp2);
                            }
                        }

                    }
                    
                    cachenode = new Node[temp2.NumberOfAvailableMoves];
                    explored++;
                    if (explored > 1000)
                    {
                        var dialog3 = new Windows.UI.Popups.MessageDialog("Over 1000 nodes have been explored in this search and total expanded nodes: " + Allnodes.Count + " , please try another kind of search !");
                        dialog3.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                        await dialog3.ShowAsync();
                        break;

                    }

                }
                int legalpath;
                if (goal(temp2) && solveable(realposition(ref temp2)))
                {

                    Stack<Node> pathstack = new Stack<Node>();
                    Allnodes[parentnode.nodeid].parentid = 0;
                    int path = 0;
                    string state;
                    int[] shownstate;
                    while (temp2.parentid != 0)
                    {
                        path++;
                        shownstate = realposition(ref temp2);
                        state = shownstate[0].ToString() + shownstate[1].ToString() + shownstate[2].ToString() + shownstate[3].ToString() + shownstate[4].ToString() + shownstate[5].ToString() + shownstate[6].ToString() + shownstate[7].ToString() + shownstate[8].ToString();
                        listview2.Items.Add(state);
                        listview1.Items.Add(path.ToString());
                        listview3.Items.Add(temp2.heuristic.ToString());
                        pathstack.Push(temp2);
                        temp2 = Allnodes[temp2.parentid];
                    }

                    legalpath = pathstack.Count;
                    int totalexpand;
                    totalexpand = frontierup.Count + frontierdown.Count + frontierright.Count + frontierleft.Count;
                    var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached after exploring " + explored + " pathcost of " + legalpath + " and expanding " + totalexpand + " nodes in graph version through depth first search");
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                    await dialog.ShowAsync();

                    MediaElement mysong = new MediaElement();
                    Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Audio");
                    Windows.Storage.StorageFile file = await folder.GetFileAsync("Tarara.mp3");
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    mysong.SetSource(stream, file.ContentType);
                    mysong.Play();


                    for (j = 0; j < legalpath; j++)
                    {

                        Setlocations(pathstack.Pop().state);
                        await Task.Delay(1300);

                    }

                }
          

            }




        }  // Applies the DFS algorithm
        private async void UCS_Click(object sender, RoutedEventArgs e)
        {
            int C, j; int superposition = 0; FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            var Allnodes = new Dictionary<double, Node>();

            Queue<Node> frontier = new Queue<Node>();
            getpositions(ref parentnode);
            setnodeid(ref parentnode);
            int[] receivesee = new int[8];
            int explored = 0;
            receivesee = realposition(ref parentnode);

            if (!solveable(realposition(ref parentnode)))
            {
                var dialog = new Windows.UI.Popups.MessageDialog("this puzzle is not solveable", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            for (C = 0; C < 8; C++)
            {
                for (j = C + 1; j < 8; j++)
                {
                    if (parentnode.state[C] == parentnode.state[j])
                    {
                        superposition++;
                    }
                }
            }  // check for superpositions
            if (superposition > 0)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("there's a super-position error in your arrangement please fix it and then try to solve", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            parentnode.PositionsTomoveTo = Action(ref parentnode);
            Node[] cachenode = new Node[parentnode.NumberOfAvailableMoves];
            Node temp = new Node();
            Node temp2 = new Node();

            Allnodes.Add(parentnode.nodeid, parentnode);
            if (goal(parentnode))
            {

                var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }

            if (superposition == 0 && !goal(parentnode))
            {
                explored = explored + 1;
                temp2 = parentnode;
                while (!goal(temp2) && solveable(realposition(ref temp2)))
                {
                    for (j = 0; j < temp2.NumberOfAvailableMoves; j++)
                    {
                        cachenode[j] = new Node();
                        copyobj(ref cachenode[j], temp2.state, temp2.nodeid, temp2.cost, moves[j]);
                        int mohm = Array.IndexOf(cachenode[j].state, temp2.PositionsTomoveTo[j]);
                        cachenode[j].state[mohm] = blankposition(ref temp2);
                        setnodeid(ref cachenode[j]);
                        if (!Allnodes.ContainsKey(cachenode[j].nodeid))
                        {
                            Allnodes.Add(cachenode[j].nodeid, cachenode[j]);
                        }
                        else
                        {

                            while (Allnodes.ContainsKey(cachenode[j].nodeid))
                            {
                                cachenode[j].nodeid *= 10;
                            }
                            Allnodes.Add(cachenode[j].nodeid, cachenode[j]);

                        }
                        frontier.Enqueue(cachenode[j]);

                    }

                    temp2 = frontier.Dequeue();
                    temp2.PositionsTomoveTo = Action(ref temp2);
                    cachenode = new Node[temp2.NumberOfAvailableMoves];
                    explored++;
                    if (explored > 1000)
                    {
                        var dialog3 = new Windows.UI.Popups.MessageDialog("Over 1000 nodes have been explored in this search and total expanded nodes: " + Allnodes.Count + " , please try another kind of search !");
                        dialog3.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                        await dialog3.ShowAsync();
                        break;
                    }
                }
                int legalpath;
                if (goal(temp2) && solveable(realposition(ref temp2)))
                {


                    Stack<Node> pathstack = new Stack<Node>();
                    Allnodes[parentnode.nodeid].parentid = 0;
                    int path = 0;
                    string state;
                    int[] shownstate;
                    while (temp2.parentid != 0)
                    {
                        path++;
                        shownstate = realposition(ref temp2);
                        state = shownstate[0].ToString() + shownstate[1].ToString() + shownstate[2].ToString() + shownstate[3].ToString() + shownstate[4].ToString() + shownstate[5].ToString() + shownstate[6].ToString() + shownstate[7].ToString() + shownstate[8].ToString();
                        listview2.Items.Add(state);
                        listview1.Items.Add(path.ToString());
                        listview3.Items.Add(temp2.heuristic.ToString());
                        pathstack.Push(temp2);
                        temp2 = Allnodes[temp2.parentid];
                    }
                    legalpath = pathstack.Count;
                    var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached after exploring " + explored + " pathcost of " + legalpath + " and expanding " + frontier.Count + " nodes in tree version through UniCost search");
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                    await dialog.ShowAsync();
                    MediaElement mysong = new MediaElement();
                    Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Audio");
                    Windows.Storage.StorageFile file = await folder.GetFileAsync("Tarara.mp3");
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    mysong.SetSource(stream, file.ContentType);
                    mysong.Play();
                    for (j = 0; j < legalpath; j++)
                    {
                        Setlocations(pathstack.Pop().state);
                        await Task.Delay(1300);
                    }
                }
            }
        }  // Applies the UCS algorithm
        private async void BFS_Click(object sender, RoutedEventArgs e)
        {
            int C, j; int superposition = 0; FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            var Allnodes = new Dictionary<double, Node>();

            Queue<Node> frontier = new Queue<Node>();
            getpositions(ref parentnode);
            setnodeid(ref parentnode);
            int[] receivesee = new int[8];
            int explored = 0;
            receivesee = realposition(ref parentnode);

            if (!solveable(realposition(ref parentnode)))
            {
                var dialog = new Windows.UI.Popups.MessageDialog("this puzzle is not solveable", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }
            for (C = 0; C < 8; C++)
            {
                for (j = C + 1; j < 8; j++)
                {
                    if (parentnode.state[C] == parentnode.state[j])
                    {
                        superposition++;
                    }
                }
            }  // check for superpositions
            if (superposition > 0)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("there's a super-position error in your arrangement please fix it and then try to solve", "Error found");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }
            parentnode.PositionsTomoveTo = Action(ref parentnode);
            Node[] cachenode = new Node[parentnode.NumberOfAvailableMoves];
            Node temp = new Node();
            Node temp2 = new Node();
            Allnodes.Add(parentnode.nodeid, parentnode);
            if (goal(parentnode))
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                await dialog.ShowAsync();
            }
            if (superposition == 0 && !goal(parentnode))
            {
                explored = explored + 1;
                temp2 = parentnode;
                while (!goal(temp2) && solveable(realposition(ref temp2)))
                {
                    for (j = 0; j < temp2.NumberOfAvailableMoves; j++)
                    {
                        cachenode[j] = new Node();
                        copyobj(ref cachenode[j], temp2.state, temp2.nodeid, temp2.cost, moves[j]);
                        int mohm = Array.IndexOf(cachenode[j].state, temp2.PositionsTomoveTo[j]);
                        cachenode[j].state[mohm] = blankposition(ref temp2);
                        setnodeid(ref cachenode[j]);
                        if (!Allnodes.ContainsKey(cachenode[j].nodeid))
                        {
                            Allnodes.Add(cachenode[j].nodeid, cachenode[j]);
                        }
                        else
                        {
                            while (Allnodes.ContainsKey(cachenode[j].nodeid))
                            {
                                cachenode[j].nodeid *= 10;
                            }
                            Allnodes.Add(cachenode[j].nodeid, cachenode[j]);
                        }
                        frontier.Enqueue(cachenode[j]);
                    }
                    temp2 = frontier.Dequeue();
                    temp2.PositionsTomoveTo = Action(ref temp2);
                    cachenode = new Node[temp2.NumberOfAvailableMoves];
                    explored++;
                    if( explored > 1000)
                    {
                        var dialog3 = new Windows.UI.Popups.MessageDialog("Over 1000 nodes have been explored in this search and total expanded nodes: " +Allnodes.Count+" , please try another kind of search !");
                        dialog3.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                        await dialog3.ShowAsync();
                        break;
                    }
                }
                int legalpath ;
                if (goal(temp2) && solveable(realposition(ref temp2)))
                {
                    Stack<Node> pathstack = new Stack<Node>();
                    Allnodes[parentnode.nodeid].parentid = 0;
                    int path = 0;
                    string state;
                    int[] shownstate;
                    while (temp2.parentid != 0)
                    {
                        path++;
                        shownstate = realposition(ref temp2);
                        state = shownstate[0].ToString() + shownstate[1].ToString() + shownstate[2].ToString() + shownstate[3].ToString() + shownstate[4].ToString() + shownstate[5].ToString() + shownstate[6].ToString() + shownstate[7].ToString() + shownstate[8].ToString();
                        listview2.Items.Add(state);
                        listview1.Items.Add(path.ToString());
                        listview3.Items.Add(temp2.heuristic.ToString());
                        pathstack.Push(temp2);
                        temp2 = Allnodes[temp2.parentid];
                    }
                    legalpath = pathstack.Count;
                    var dialog = new Windows.UI.Popups.MessageDialog("Goal has been reached after exploring " + explored + " pathcost of " + legalpath + " and expanding " + frontier.Count + " nodes in tree version through breadth first search");
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                    await dialog.ShowAsync();
                    MediaElement mysong = new MediaElement();
                    Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Audio");
                    Windows.Storage.StorageFile file = await folder.GetFileAsync("Tarara.mp3");
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    mysong.SetSource(stream, file.ContentType);
                    mysong.Play();
                    for (j = 0; j < legalpath; j++)
                    { 
                        Setlocations(pathstack.Pop().state);
                        await Task.Delay(1300);
                        
                    }
                }
            }
        }  // Applies the BFS algorithm
        private void pieceOne_Click(object sender, RoutedEventArgs e)
        {
            clicked = 1;
        }
        private void piecetwo_Click(object sender, RoutedEventArgs e)
        {
            clicked = 2;
        }
        private void piecethree_Click(object sender, RoutedEventArgs e)
        {
            clicked = 3;
        }
        private void piecefour_Click(object sender, RoutedEventArgs e)
        {
            clicked = 4;
        }
        private void piecefive_Click(object sender, RoutedEventArgs e)
        {
            clicked = 5;
        }
        private void piecesix_Click(object sender, RoutedEventArgs e)
        {
            clicked = 6;
        }
        private void pieceSeven_Click(object sender, RoutedEventArgs e)
        {
            clicked = 7;
        }
        private void pieceeight_Click(object sender, RoutedEventArgs e)
        {
            clicked = 8;
        }
        private void move_up_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            if (Grid.GetRow(buttons[clicked - 1]) > 0)
            {
                Grid.SetRow(buttons[clicked - 1], Grid.GetRow(buttons[clicked - 1]) - 1);
            }

        }
        private void move_down_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            if (Grid.GetRow(buttons[clicked - 1]) < 2)
            {
                Grid.SetRow(buttons[clicked - 1], Grid.GetRow(buttons[clicked - 1]) + 1);
            }
        }
        private void move_left_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            if (Grid.GetColumn(buttons[clicked - 1]) > 0)
            {
                Grid.SetColumn(buttons[clicked - 1], Grid.GetColumn(buttons[clicked - 1]) - 1);
            }
        }
        private void move_right_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement[] buttons = { pieceOne, piecetwo, piecethree, piecefour, piecefive, piecesix, pieceSeven, pieceeight };
            if (Grid.GetColumn(buttons[clicked - 1]) < 2)
            {
                Grid.SetColumn(buttons[clicked - 1], Grid.GetColumn(buttons[clicked - 1]) + 1);
            }
        }
    }
}
