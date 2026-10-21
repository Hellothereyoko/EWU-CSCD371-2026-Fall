namespace Dag



{

    /// <summary>
    /// A generic class representing a Directed Acyclic Graph (DAG).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Dag<T> 
    {
        //public List<T> Nodes { get; private set; } 

        AddNode(T value)
        {
            //Add Node to the graph

            //Check if the node already exists in the graph
        }



        AddDependency(T dependsOn, T dependent)
        {
            //Both nodes must exist in the graph before adding a dependency

            //Add directed edge from dependsOn to dependent

            //Do not add dup edges

            //Throw InvalidOperationException if adding the edge would create a cycle in the graph

        }
        


        GetDependencies(T value)
        {
            //return the nodes that `value` **directly** depends on.

            //Throw InvalidOperationException if the node does not exist in the graph
        }



        GetDependents(T value)
        {
            //return the nodes that **directly** depend on `value`.

            //Throw InvalidOperationException if the node does not exist in the graph
        }   
    }
}