/// @file AtomAttribution.cs
/// @brief Details to be specified
/// @author FvNano/LBT team
/// @author Marc Baaden <baaden@smplinux.de>
/// @date   2013-4
///
/// Copyright Centre National de la Recherche Scientifique (CNRS)
///
/// contributors :
/// FvNano/LBT team, 2010-13
/// Marc Baaden, 2010-13
///
/// baaden@smplinux.de
/// http://www.baaden.ibpc.fr
///
/// This software is a computer program based on the Unity3D game engine.
/// It is part of UnityMol, a general framework whose purpose is to provide
/// a prototype for developing molecular graphics and scientific
/// visualisation applications. More details about UnityMol are provided at
/// the following URL: "http://unitymol.sourceforge.net". Parts of this
/// source code are heavily inspired from the advice provided on the Unity3D
/// forums and the Internet.
///
/// This software is governed by the CeCILL-C license under French law and
/// abiding by the rules of distribution of free software. You can use,
/// modify and/or redistribute the software under the terms of the CeCILL-C
/// license as circulated by CEA, CNRS and INRIA at the following URL:
/// "http://www.cecill.info".
/// 
/// As a counterpart to the access to the source code and rights to copy, 
/// modify and redistribute granted by the license, users are provided only 
/// with a limited warranty and the software's author, the holder of the 
/// economic rights, and the successive licensors have only limited 
/// liability.
///
/// In this respect, the user's attention is drawn to the risks associated 
/// with loading, using, modifying and/or developing or reproducing the 
/// software by the user in light of its specific status of free software, 
/// that may mean that it is complicated to manipulate, and that also 
/// therefore means that it is reserved for developers and experienced 
/// professionals having in-depth computer knowledge. Users are therefore 
/// encouraged to load and test the software's suitability as regards their 
/// requirements in conditions enabling the security of their systems and/or 
/// data to be ensured and, more generally, to use and operate it in the 
/// same conditions as regards security.
///
/// The fact that you are presently reading this means that you have had 
/// knowledge of the CeCILL-C license and that you accept its terms.
///
/// $Id: AtomAttribution.cs 213 2013-04-06 21:13:42Z baaden $
///
/// References : 
/// If you use this code, please cite the following reference : 	
/// Z. Lv, A. Tek, F. Da Silva, C. Empereur-mot, M. Chavent and M. Baaden:
/// "Game on, Science - how video game technology may help biologists tackle
/// visualization challenges" (2013), PLoS ONE 8(3):e57990.
/// doi:10.1371/journal.pone.0057990
///
/// If you use the HyperBalls visualization metaphor, please also cite the
/// following reference : M. Chavent, A. Vanel, A. Tek, B. Levy, S. Robert,
/// B. Raffin and M. Baaden: "GPU-accelerated atom and dynamic bond visualization
/// using HyperBalls, a unified algorithm for balls, sticks and hyperboloids",
/// J. Comput. Chem., 2011, 32, 2924
///

namespace UI
{
	using UnityEngine;
	using System.Collections;
	
	/** Make the 
	 * 
	 * 
	 * 
	 */
	public class AtomAttribution 
	{
		public Rect addRect;
    	public Rect subRect;
	    private int data=0;
		
		/** Classic Constructor.
		 * Set data.
		 */
		
		public AtomAttribution()
		{
			
		}
		
		/** 
		 * 
		 * Unity3D Doc : <BR>
		 * <A HREF="http://docs.unity3d.com/Documentation/ScriptReference/Rect.html">Rect</A><BR>
		 * <A HREF="http://docs.unity3d.com/Documentation/ScriptReference/GUILayout.html">GUILayout</A><BR>
		 * <A HREF="http://docs.unity3d.com/Documentation/ScriptReference/MonoBehaviour.html">GUI</A>
		 */
		public  void Display(string name,ref string scale, string color,string number,ref bool displayColors,ref int AtomType)
		{
			if(name.Equals("Oxygen"))
				{
			addRect = new Rect(115, 132, 20, 20);
    		subRect = new Rect(65, 132, 20, 20);
				}
				
				else if(name.Equals("Sulphur"))
				{
			addRect = new Rect(115, 156, 20, 20);
    		subRect = new Rect(65, 156, 20, 20);
				}
				
				else if(name.Equals("Carbon"))
				{
			addRect = new Rect(115, 85, 20, 20);
    		subRect = new Rect(65, 85, 20, 20);
				}
				
				else if(name.Equals("Nitrogen"))
				{
			addRect = new Rect(115, 108, 20, 20);
    		subRect = new Rect(65, 108, 20, 20);
				}
				
				else if(name.Equals("Phosphor"))
				{
			addRect = new Rect(115, 178, 20, 20);
    		subRect = new Rect(65, 178, 20, 20);
				}
				
				else if(name.Equals("Hydrogen"))
				{
			addRect = new Rect(115, 202, 20, 20);
    		subRect = new Rect(65, 202, 20, 20);
				}
				
				else
				{
			addRect = new Rect(115, 226, 20, 20);
    		subRect = new Rect(65, 226, 20, 20);
				}
	
			GUILayout.BeginHorizontal();
			GUILayout.Label(name,GUILayout.MaxWidth(70));	
		if (GUI.RepeatButton(subRect, "-"))
        {
            int.TryParse(scale, out data);
            data-=10;
            scale = data.ToString();
        }

       			scale=GUILayout.TextField(scale,3,GUILayout.Width(30));
		 if (GUI.RepeatButton(addRect, "+"))
        {
            int.TryParse(scale, out data);
            data+=10;
            scale = data.ToString();
        }
	
			GUILayout.Space(20);
			if(GUILayout.Button(color,GUILayout.Width(60)))
			{
				displayColors=true;
				if(name.Equals("Oxygen"))
				{
					AtomType=0;
				}
				
				else if(name.Equals("Sulphur"))
				{
					AtomType=1;
				}
				
				else if(name.Equals("Carbon"))
				{
					AtomType=2;
				}
				
				else if(name.Equals("Nitrogen"))
				{
					AtomType=3;
				}
				
				else if(name.Equals("Phosphor"))
				{
					AtomType=4;
				}
				
				else if(name.Equals("Hydrogen"))
				{
					AtomType=5;
				}
				
				else if(name.Equals("Unknown"))
				{
					AtomType=6;
				}
				
				
			}
			GUILayout.Space(40);		
			GUILayout.Label(number);
			GUILayout.EndHorizontal();
		}
		
		
		
	}
	
	
}
