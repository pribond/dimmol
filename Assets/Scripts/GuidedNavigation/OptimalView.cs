// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using Molecule.Model;
using System.Collections;
using System.Collections.Generic;
using BenTools.Mathematics;
using System.Linq;
using System.IO;

namespace UI
{
		public class OptimalView
		{
				/// <summary>
				/// Calculate distance between two vectors of length 2.
				/// </summary>
				public static double Distance2f(Vector pointA, Vector pointB)
				{
					return Math.Sqrt(Math.Pow(pointA[0]-pointB[0], 2) + Math.Pow(pointA[1]-pointB[1], 2));
				}
				/// <summary>
				/// Calculate distance between two float array of length 3.
				/// </summary>
				public static double Distance3f(float[] pointA, float[] pointB)
				{
					return Math.Sqrt(Math.Pow(pointA[0]-pointB[0], 2) + Math.Pow(pointA[1]-pointB[1], 2) + Math.Pow(pointA[2]-pointB[2], 2));
				}
				/// <summary>
				/// Get the position with the largest view cone on a specific target and with respect to an input radius
				/// </summary>
				public static void GetOptimalPosition (float[] target, float radius)
				{
					List<float[]> neighbor_coords = new List<float[]>();
					ArrayList neighbor_theta_phi = new ArrayList();
					ArrayList neighbor_correspondance = new ArrayList();
					float min_theta = 1000.0f;
					float max_theta = 0.0f;
					float min_phi = 1000.0f;
					float max_phi = 0.0f;
			
					// Get all the atoms inside the sphere centered on the target and of the input radius, translate them into polar coordinates
					for(int i=0; i<MoleculeModel.atomsLocationlist.Count; i++)
					{
						double dist = Distance3f(MoleculeModel.atomsLocationlist[i],target);
						if (dist < radius)
						{
							neighbor_coords.Add(MoleculeModel.atomsLocationlist[i]);
							float[] atom = new float[3];
							atom[0] = MoleculeModel.atomsLocationlist[i][0] - target[0];
							atom[1] = MoleculeModel.atomsLocationlist[i][1] - target[1];
							atom[2] = MoleculeModel.atomsLocationlist[i][2] - target[2];
							float theta = (float) Math.Acos(atom[2] / dist);
							float phi = (float) Math.Atan2(atom[1], atom[0]);
							Vector theta_phi = new Vector(2);
							theta_phi[0] = theta;
							theta_phi[1] = phi;
							neighbor_theta_phi.Add(theta_phi);
							neighbor_correspondance.Add(i);
//							Debug.Log (theta_phi);
							theta_phi = new Vector(2);
							theta_phi[0] = theta + (float) Math.PI / 2;
							theta_phi[1] = phi + (float) Math.PI;
							neighbor_theta_phi.Add(theta_phi);
//							Debug.Log (theta_phi);
							theta_phi = new Vector(2);
							theta_phi[0] = theta + (float) Math.PI / 2;
							theta_phi[1] = phi;
							neighbor_theta_phi.Add(theta_phi);
//							Debug.Log (theta_phi);
							theta_phi = new Vector(2);
							theta_phi[0] = theta;
							theta_phi[1] = phi + (float) Math.PI;
							neighbor_theta_phi.Add(theta_phi);
//							Debug.Log (theta_phi);
							if (theta + (float) Math.PI / 2 > max_theta)
								max_theta = theta + (float) Math.PI / 2;
							if (theta < min_theta)
								min_theta = theta;
							if (phi + (float) Math.PI > max_phi)
								max_phi = phi + (float) Math.PI;
							if (phi < min_phi)
								min_phi = phi;
						}
					}

//					Debug.Log (neighbor_theta_phi.GetEnumerator().Current[0]+" "+neighbor_theta_phi.GetEnumerator().Current[1]);
//					Debug.Log (neighbor_theta_phi[1][0]+" "+neighbor_theta_phi[1][1]);
//					Debug.Log (neighbor_theta_phi[2][0]+" "+neighbor_theta_phi[2][1]);
//					Debug.Log (neighbor_theta_phi[3][0]+" "+neighbor_theta_phi[3][1]);
			
					StreamWriter sw = new StreamWriter(@"/Users/trellet/Dev/UnityMol_svn/trunk/Assets/neighbors.txt");
					
					
					foreach (Vector neighbor in neighbor_theta_phi)
					{
						sw.WriteLine(""+neighbor[0]+" "+neighbor[1]);
					}
					sw.Close ();

//					int length = neighbor_theta_phi.Count;
//					for(int i=0; i<length; i++)
//					{
//						Vector theta_phi = new Vector(2);
//						theta_phi[0] = neighbor_theta_phi[i][0]+ (float) Math.PI;
//						theta_phi[1] = neighbor_theta_phi[i][1]+2*(float) Math.PI;
//						neighbor_theta_phi.Add (theta_phi);
//						theta_phi[0] = neighbor_theta_phi[i][0]+(float) Math.PI;
//						theta_phi[1] = neighbor_theta_phi[i][1];
//						neighbor_theta_phi.Add (theta_phi);
//						theta_phi[0] = neighbor_theta_phi[i][0];
//						theta_phi[1] = neighbor_theta_phi[i][1]+2*(float) Math.PI;
//						neighbor_theta_phi.Add (theta_phi);
//					}
					Debug.Log("Nb of neighbors: "+neighbor_theta_phi.Count);
					Debug.Log("Min/max theta/phi: "+min_theta+" "+max_theta+" "+min_phi+" "+max_phi);
					
					// Compute the Voronoi graph from the neighbors polar coordinates
					VoronoiGraph result = Fortune.ComputeVoronoiGraph(neighbor_theta_phi);
					MoleculeModel.atomsLocationlist.OrderBy(x=>x[0]);
					Debug.Log (result.Vertizes.Count);
					
					StreamWriter sw2 = new StreamWriter(@"/Users/trellet/Dev/UnityMol_svn/trunk/Assets/vertices.txt");
					BenTools.Data.HashSet temp = new BenTools.Data.HashSet();
					
					foreach (Vector vert in result.Vertizes)
					{
						if (vert[0] > min_theta && vert[0] < max_theta && vert[1] < max_phi && vert[1] > min_phi)
						{
							sw2.WriteLine(""+vert[0]+" "+vert[1]);
							temp.Add(vert);
						}
					}
					sw2.Close ();
					
					result.Vertizes = temp;
					
					
					
					//double min_dist = 1000.0;
					double max_dist = 0.0;
					float[] best_pos = new float[2];
					//float[] best_point = new float[2];
					//float[] vertex = new float[2];
					//float[] point = new float[2];
			//					Vector vert = new Vector();
					//int c = 0;
					double distance = 0.0;
					Dictionary<double, float[]> vertices = new Dictionary<double, float[]>();
					
					
					// Find the largest distance between each vertex and the closest point to each of them
					//// 1st METHOD (faster, use the edges that contain point information)
					foreach (VoronoiEdge edge in result.Edges)
					{
						//min_dist = 1000.0;
						
						if (edge.VVertexA[0] > min_theta && edge.VVertexA[0] < max_theta && edge.VVertexA[1] < max_phi && edge.VVertexA[1] > min_phi)
						{
							distance = Distance2f(edge.VVertexA, edge.LeftData);
							float[] t = new float[2];
							t[0] = (float) edge.VVertexA[0];
							t[1] = (float) edge.VVertexA[1];
							vertices[distance] = t;
							if (distance > max_dist)
							{
								max_dist = distance;
								best_pos[0] = (float) edge.VVertexA[0];
								best_pos[1] = (float) edge.VVertexA[1];
							}
						
						}
						if (edge.VVertexB[0] > min_theta && edge.VVertexB[0] < max_theta && edge.VVertexB[1] < max_phi && edge.VVertexB[1] > min_phi)
						{
							distance = Distance2f(edge.VVertexB, edge.LeftData);
							float[] t = new float[2];
							t[0] = (float) edge.VVertexA[0];
							t[1] = (float) edge.VVertexA[1];
							vertices[distance] = t;
							if (distance > max_dist)
							{
								max_dist = distance;
								best_pos[0] = (float) edge.VVertexB[0];
								best_pos[1] = (float) edge.VVertexB[1];
							}
						}
					}
					
					var list = vertices.Keys.ToList();
					list.Sort();
					float[] cartesian = new float[3];
					for(int i=list.Count-1; i>list.Count-8; i--)
					{
						//Debug.Log(list[i]+": "+vertices[list[i]][0]+" "+vertices[list[i]][1]);
						cartesian[0] = (radius * (float) Math.Sin(vertices[list[i]][0]) * (float) Math.Cos(vertices[list[i]][1])) + target[0];
						cartesian[1] = (radius * (float) Math.Sin(vertices[list[i]][0]) * (float) Math.Sin(vertices[list[i]][1])) + target[1];
						cartesian[2] = (radius * (float) Math.Cos(vertices[list[i]][0])) + target[2];
						Debug.Log (list[i]+": "+cartesian[0]+" "+cartesian[1]+" "+cartesian[2]);
					}

					////// 2nd METHOD (slower, all vertices vs all points)
//					foreach (Vector vert in result.Vertizes)
//					{
//						min_dist = 1000.0;
//					
//						foreach (Vector neighbor in neighbor_theta_phi)
//						{	
////							vertices[0] = (float) vert[0];
////							vertices[1] = (float) vert[1];
//							
//							double dist = Distance2f(vert, neighbor);
//							
//							if (dist < min_dist)
//							{
//								min_dist = dist;
//								point[0] = (float) neighbor[0];
//								point[1] = (float) neighbor[1];
//							}
//						}
//						if (min_dist > max_dist)
//						{
//							max_dist = min_dist;
//							best_pos[0] = (float) vert[0];
//							best_pos[1] = (float) vert[1];
//							best_point[0] = point[0];
//							best_point[1] = point[1];
//						}
//						
//					}
					Debug.Log ("Maximum distance: "+max_dist);
					Debug.Log("Theta and phi: "+best_pos[0]+" "+best_pos[1]);
					float[] best_pos_cart = new float[3];
					//float[] best_pos_cart2 = new float[3];
					//float[] best_pos_cart3 = new float[3];
					//float[] best_pos_cart4 = new float[3];
			
					// Convert polar coordinates of the best position to cartesian ones + shift to molecule system coordinates
					best_pos_cart[0] = (radius * (float) Math.Sin(best_pos[0]) * (float) Math.Cos(best_pos[1])) + target[0];
					best_pos_cart[1] = (radius * (float) Math.Sin(best_pos[0]) * (float) Math.Sin(best_pos[1])) + target[1];
					best_pos_cart[2] = (radius * (float) Math.Cos(best_pos[0])) + target[2];
					Debug.Log("Best position: "+best_pos_cart[0]+" "+best_pos_cart[1]+" "+best_pos_cart[2]);
//					best_pos_cart2[0] = (radius * (float) Math.Sin(best_pos[0]-Math.PI) * (float) Math.Cos(best_pos[1])) + target[0];
//					best_pos_cart2[1] = (radius * (float) Math.Sin(best_pos[0]-Math.PI) * (float) Math.Sin(best_pos[1])) + target[1];
//					best_pos_cart2[2] = (radius * (float) Math.Cos(best_pos[0]-Math.PI)) + target[2];
//					best_pos_cart3[0] = (radius * (float) Math.Sin(best_pos[0]-Math.PI) * (float) Math.Cos(best_pos[1]-2*Math.PI)) + target[0];
//					best_pos_cart3[1] = (radius * (float) Math.Sin(best_pos[0]-Math.PI) * (float) Math.Sin(best_pos[1]-2*Math.PI)) + target[1];
//					best_pos_cart3[2] = (radius * (float) Math.Cos(best_pos[0]-Math.PI)) + target[2];
//					best_pos_cart4[0] = (radius * (float) Math.Sin(best_pos[0]) * (float) Math.Cos(best_pos[1]-2*Math.PI)) + target[0];
//					best_pos_cart4[1] = (radius * (float) Math.Sin(best_pos[0]) * (float) Math.Sin(best_pos[1]-2*Math.PI)) + target[1];
//					best_pos_cart4[2] = (radius * (float) Math.Cos(best_pos[0])) + target[2];
//					Debug.Log("Best position2: "+best_pos_cart2[0]+" "+best_pos_cart2[1]+" "+best_pos_cart2[2]);
//					Debug.Log("Best position3: "+best_pos_cart3[0]+" "+best_pos_cart3[1]+" "+best_pos_cart3[2]);
//					Debug.Log("Best position4: "+best_pos_cart4[0]+" "+best_pos_cart4[1]+" "+best_pos_cart4[2]);
			
					// Place the camera at the new best position and make it face the target
					UIData.Instance.optim_view = true;
					MaxCameraData.Instance.optim_target = new Vector3(target[0], target[1], target[2]);
					MaxCameraData.Instance.optim_cam_position = new Vector3(best_pos_cart[0], best_pos_cart[1], best_pos_cart[2]);
					GameObject camera = GameObject.Find("LoadBox");
					UIData.Instance.optim_view_start_point = camera.transform.position;
					UIData.Instance.start_time = Time.time;
					//camera.transform.position = new Vector3(best_pos_cart[0], best_pos_cart[1], best_pos_cart[2]);
//					Wait();
//					camera.transform.position = new Vector3(best_pos_cart2[0], best_pos_cart2[1], best_pos_cart2[2]);
//					Wait();
//					camera.transform.position = new Vector3(best_pos_cart3[0], best_pos_cart3[1], best_pos_cart3[2]);
//					Wait();
//					camera.transform.position = new Vector3(best_pos_cart4[0], best_pos_cart4[1], best_pos_cart4[2]);
					//MaxCameraData.Instance.ghost_target = GameObject.Find("Target");
					
//					camera.transform.LookAt(ghost_target.transform);
				//result.Vertizes
				}
				public static IEnumerator Wait()
				{
					float timetowait = 0.5f;
					float incrementation = 0.1f;
					while(timetowait > 0)
					{
						yield return new WaitForSeconds(incrementation);
						timetowait -= incrementation;
					} 
				}
		}
}

