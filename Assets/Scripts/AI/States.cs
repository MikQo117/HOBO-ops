using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using StateStuff;

namespace StateStuff
{
    public class ScavengeState : State<HoboController>
    {

        private Vector2[]                  shortestPath;
        private TrashSpawn                 nearestSpawn = new TrashSpawn();
        private List<SpawnData>            spawns = new List<SpawnData>();
        private int                        requestsSent = 0;

        //Sub-state variables
        private int                        subState = 0;

        public void PathEndReached()
        {
            // The end of the path has been reached, advance to next sub-state
            subState = 2;
        }

        private void ResetVariables()
        {
            requestsSent = 0;
            requestIndex = 0;
            subState = 0;
            spawns.Clear();
            shortestPath = new Vector2[0];
            nearestSpawn = null;
        }

        private void ResetSubStateVariables()
        {
            requestsSent = 0;
            subState = 0;
            shortestPath = new Vector2[0];
            nearestSpawn = null;
        }

        private void ResetPaths()
        {
            foreach (SpawnData item in spawns)
            {
                item.Path = null;
            }
        }

        public override void EnterState(HoboController owner)
        {
            ResetVariables();
            //Debug.Log("Entering scavenge state");
            GetTrashCans();
            SendPathRequests(owner.transform.position);
        }

        bool stateActive = false;

        public override void UpdateState(HoboController owner)
        {
            stateActive = owner.StateMachine.currentState == this;
            switch (subState)
            {
                default:
                    break;
                case 0:
                    //Paths not received
                    //Debug.Log("Waiting for paths");
                    break;
                case 1:
                    //Paths received
                    //Move
                    if (!owner.MovingToTarget)
                    {
                        //Debug.Log("Movement started");
                        owner.Grid.Path = shortestPath;
                        owner.StartMovement(shortestPath);
                    }
                    else
                    {
                        //Debug.Log("Waiting for movement to end");
                    }
                    break;
                case 2:
                    //Target reached, stop movement and gather
                    //Debug.Log("Target reached");
                    spawns.Remove(spawns.Find(x => x.ActiveTarget == true));
                    owner.tryInteract = true;
                    ResetPaths();
                    owner.EatUntilSatisfied();
                    if (spawns.Count <= 0)
                    {
                        owner.StateMachine.ChangeState(owner.idleState); 
                    }
                    ResetSubStateVariables();
                    SendPathRequests(owner.transform.position);
                    subState = 0;
                    break;
            }
        }


        public override void ExitState(HoboController owner)
        {
            Debug.Log("Exiting scavenge state");
            //Reset variables
            ResetVariables();
            stateActive = false;
        }


        private void GetTrashCans()
        {
            if (GameManager.Instance.interactables.Count > 0)
            {
                foreach (IInteractable item in GameManager.Instance.interactables)
                {
                    if (item is TrashSpawn)
                    {
                        spawns.Add(new SpawnData((TrashSpawn)item, null, 0));
                    }
                } 
            }
            else
            {
                Debug.Log("Interactables not populated");
            }
        }
        private void SendPathRequests(Vector3 startPosition)
        {
            foreach (SpawnData item in spawns)
            {
                PathRequestManager.RequestPath(startPosition, item.Target.transform.position, OnPathStuff);
                requestsSent++;
            }
        }

        private int requestIndex = 0;
        public void OnPathStuff(Vector2[] newPath, bool pathSuccess, int pathLength)
        {
            if (pathSuccess && stateActive)
            {
                spawns[requestIndex].Path = newPath;
                spawns[requestIndex].PathLength = pathLength; 
                requestIndex++;

                if (PathsPopulated())
                {
                    shortestPath = spawns.OrderBy(spawn => spawn.PathLength).First().SetAsCurrent(); //Vittumikäsäätö
                    //shortestPath = spawns.Aggregate((l, r) => l.PathLength < r.PathLength ? l : r).SetAsCurrent(); //Vittumikäsäätö
                    //shortestPath = spawns.Where(x => x.PathLength == paths.Select(k => paths[k.Key]).Min());
                    subState = 1;
                    requestIndex = 0;
                }
            }
        }

        private bool PathsPopulated()
        {
            int count = 0;

            foreach (SpawnData item in spawns)
            {
                if (item.Path != null)
                {
                    count++;
                }
            }

            if (count >= requestsSent)
            {
                return true;
            }
            return false;

        }
    }

    public class MovementState : State<PedestrianController>
    {

        private Vector2[] shortestPath;
        private TrashSpawn nearestSpawn;
        private List<LocationData> locations = new List<LocationData>();
        private int requestsSent = 0;
        private float waitAtEndTime = 0;
        private float waitTimer = 0;
        private float minWaitTime = 3;
        private float maxWaitTime = 10;

        //Sub-state variables
        private int subState = 0;

        public void PathEndReached()
        {
            // The end of the path has been reached, advance to next sub-state
            subState = 2;
        }

        private void ResetVariables()
        {
            waitAtEndTime = Random.Range(minWaitTime, maxWaitTime);
            waitTimer = 0;
            requestsSent = 0;
            requestIndex = 0;
            subState = 0;
            locations.Clear();
            shortestPath = new Vector2[0];
            nearestSpawn = null;
        }

        private void ResetSubStateVariables()
        {
            waitAtEndTime = Random.Range(minWaitTime, maxWaitTime);
            waitTimer = 0;
            requestsSent = 0;
            subState = 0;
            shortestPath = new Vector2[0];
            nearestSpawn = null;
        }

        private void ResetPaths()
        {
            foreach (LocationData item in locations)
            {
                item.Path = null;
            }
        }

        public override void EnterState(PedestrianController owner)
        {
            ResetVariables();
            //Debug.Log("Entering scavenge state");
            GetLocations();
            SendPathRequests(owner.transform.position);
        }

        bool stateActive = false;

        public override void UpdateState(PedestrianController owner)
        {
            stateActive = owner.StateMachine.currentState == this;
            switch (subState)
            {
                default:
                    break;
                case 0:
                    //Paths not received
                    //Debug.Log("Waiting for paths");
                    break;
                case 1:
                    //Paths received
                    //Move
                    if (!owner.MovingToTarget)
                    {
                        //Debug.Log("Movement started");
                        owner.StartMovement(shortestPath);
                    }
                    else
                    {
                        //Debug.Log("Waiting for movement to end");
                    }
                    break;
                case 2:
                    if (Wait())
                    {
                        subState = 3;
                    }
                    break;
                case 3:
                    //Target reached, stop movement and gather
                    //Debug.Log("Target reached");
                    locations.Remove(locations.Find(x => x.ActiveTarget == true));
                    owner.tryInteract = true;
                    ResetPaths();
                    if (locations.Count <= 0)
                    {
                        //Go destroy self state?
                        //owner.StateMachine.ChangeState(owner.idleState);
                        Debug.Log("End of movement state");
                    }
                    ResetSubStateVariables();
                    SendPathRequests(owner.transform.position);
                    subState = 0;
                    break;
                
            }
        }

        private bool Wait()
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtEndTime)
            {
                waitTimer = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void ExitState(PedestrianController owner)
        {
            Debug.Log("Exiting scavenge state");
            //Reset variables
            ResetVariables();
            stateActive = false;
        }


        private void GetLocations()
        {
            List<PedestrianTarget> targets = GameManager.Instance.GetPedestrianTargets;
            if (targets.Count > 0)
            {
                foreach (PedestrianTarget item in targets)
                {
                    locations.Add(new LocationData(item, null, 0));
                }
            }
            else
            {
                Debug.Log("Locations not populated");
            }
        }
        private void SendPathRequests(Vector3 startPosition)
        {
            foreach (LocationData item in locations)
            {
                PathRequestManager.RequestPath(startPosition, item.Target.transform.position, OnPathStuff);
                requestsSent++;
            }
        }

        private int requestIndex = 0;
        public void OnPathStuff(Vector2[] newPath, bool pathSuccess, int pathLength)
        {
            if (pathSuccess && stateActive)
            {
                locations[requestIndex].Path = newPath;
                locations[requestIndex].PathLength = pathLength;
                requestIndex++;

                if (PathsPopulated())
                {
                    //shortestPath = locations.OrderBy(spawn => spawn.PathLength).First().SetAsCurrent(); //Vittumikäsäätö
                    shortestPath = locations[Random.Range(0, locations.Count-1)].SetAsCurrent();
                    subState = 1;
                    requestIndex = 0;
                }
            }
        }

        private bool PathsPopulated()
        {
            int count = 0;

            foreach (LocationData item in locations)
            {
                if (item.Path != null)
                {
                    count++;
                }
            }

            if (count >= requestsSent)
            {
                return true;
            }
            return false;

        }
    
        public class LocationData
        {
            public PedestrianTarget Target;
            public Vector2[] Path;
            public int PathLength;
            private bool activeTarget;

            public LocationData(PedestrianTarget target, Vector2[] path, int pathLength)
            {
                Target = target;
                Path = path;
                PathLength = pathLength;
                activeTarget = false;
            }

            public bool ActiveTarget
            {
                get { return activeTarget; }
                set { activeTarget = value; }
            }

            public Vector2[] SetAsCurrent()
            {
                activeTarget = true;
                return Path;
            }
        }
    }


    public class SpawnData
    {
        public TrashSpawn Target;
        public Vector2[]  Path;
        public int        PathLength;
        private bool      activeTarget;

        public SpawnData(TrashSpawn target, Vector2[] path, int pathLength)
        {
            Target = target;
            Path = path;
            PathLength = pathLength;
            activeTarget = false;
        }

        public bool ActiveTarget
        {
            get { return activeTarget; }
            set { activeTarget = value; }
        }

        public Vector2[] SetAsCurrent()
        {
            activeTarget = true;
            return Path;
        }
    }


    public class IdleState : State<HoboController>
    {
        public override void EnterState(HoboController owner)
        {
            //Debug.Log("Entering idle state");
            owner.StateMachine.ChangeState(owner.scavengeState);
        }

        public override void ExitState(HoboController owner)
        {
            //Debug.Log("Exiting idle state");
        }

        public override void UpdateState(HoboController owner)
        {
            
        }
    }

    public class BackToMovement : State<PedestrianController>
    {
        public override void EnterState(PedestrianController owner)
        {
            //Debug.Log("Entering idle state");
            owner.StateMachine.ChangeState(owner.movementState);
        }

        public override void ExitState(PedestrianController owner)
        {
            //Debug.Log("Exiting idle state");
        }

        public override void UpdateState(PedestrianController owner)
        {

        }
    }
}