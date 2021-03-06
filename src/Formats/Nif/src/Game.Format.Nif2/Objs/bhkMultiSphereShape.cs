/* Copyright (c) 2006, NIF File Format Library and Tools */
// THIS FILE WAS AUTOMATICALLY GENERATED.  DO NOT EDIT! //
// To change this file, alter the generate_cs.py Python script.
//-----------------------------------NOTICE----------------------------------//
// Only add custom code in the designated areas to preserve between builds   //
//-----------------------------------NOTICE----------------------------------//

using System;
using System.IO;
using System.Collections.Generic;

namespace Niflib
{
	/*! Unknown. */
	public class bhkMultiSphereShape : bhkSphereRepShape
	{
		// Definition of TYPE constant
		public static readonly Type_ TYPE = new Type_("bhkMultiSphereShape", bhkSphereRepShape.TYPE);

		/*! Unknown. */
		internal float unknownFloat1;
		/*! Unknown. */
		internal float unknownFloat2;
		/*! The number of spheres in this multi sphere shape. */
		internal uint numSpheres;
		/*! This array holds the spheres which make up the multi sphere shape. */
		internal IList<NiBound> spheres;
		public bhkMultiSphereShape()
		{
			unknownFloat1 = 0.0f;
			unknownFloat2 = 0.0f;
			numSpheres = (uint)0;
		}

		/*! Used to determine the type of a particular instance of this object. \return The type constant for the actual type of the object. */
		public override Type_ GetType() => TYPE;

		/*!
		 * A factory function used during file reading to create an instance of this type of object.
		 * \return A pointer to a newly allocated instance of this type of object.
		 */
		public static NiObject Create() => new bhkMultiSphereShape();

		/*! NIFLIB_HIDDEN function.  For internal use only. */
		internal override void Read(IStream s, List<uint> link_stack, NifInfo info)
		{
			base.Read(s, link_stack, info);
			Nif.NifStream(out unknownFloat1, s, info);
			Nif.NifStream(out unknownFloat2, s, info);
			Nif.NifStream(out numSpheres, s, info);
			spheres = new NiBound[numSpheres];
			for (var i3 = 0; i3 < spheres.Count; i3++)
			{
				Nif.NifStream(out spheres[i3].center, s, info);
				Nif.NifStream(out spheres[i3].radius, s, info);
			}
		}

		/*! NIFLIB_HIDDEN function.  For internal use only. */
		internal override void Write(OStream s, Dictionary<NiObject, uint> link_map, List<NiObject> missing_link_stack, NifInfo info)
		{
			base.Write(s, link_map, missing_link_stack, info);
			numSpheres = (uint)spheres.Count;
			Nif.NifStream(unknownFloat1, s, info);
			Nif.NifStream(unknownFloat2, s, info);
			Nif.NifStream(numSpheres, s, info);
			for (var i3 = 0; i3 < spheres.Count; i3++)
			{
				Nif.NifStream(spheres[i3].center, s, info);
				Nif.NifStream(spheres[i3].radius, s, info);
			}
		}

		/*!
		 * Summarizes the information contained in this object in English.
		 * \param[in] verbose Determines whether or not detailed information about large areas of data will be printed cs.
		 * \return A string containing a summary of the information within the object in English.  This is the function that Niflyze calls to generate its analysis, so the output is the same.
		 */
		public override string AsString(bool verbose = false)
		{
			var s = new System.Text.StringBuilder();
			var array_output_count = 0U;
			s.Append(base.AsString());
			numSpheres = (uint)spheres.Count;
			s.AppendLine($"      Unknown Float 1:  {unknownFloat1}");
			s.AppendLine($"      Unknown Float 2:  {unknownFloat2}");
			s.AppendLine($"      Num Spheres:  {numSpheres}");
			array_output_count = 0;
			for (var i3 = 0; i3 < spheres.Count; i3++)
			{
				if (!verbose && (array_output_count > Nif.MAXARRAYDUMP))
				{
					s.AppendLine("<Data Truncated. Use verbose mode to see complete listing.>");
					break;
				}
				s.AppendLine($"        Center:  {spheres[i3].center}");
				s.AppendLine($"        Radius:  {spheres[i3].radius}");
			}
			return s.ToString();
		}

		/*! NIFLIB_HIDDEN function.  For internal use only. */
		internal override void FixLinks(Dictionary<uint, NiObject> objects, List<uint> link_stack, List<NiObject> missing_link_stack, NifInfo info)
		{
			base.FixLinks(objects, link_stack, missing_link_stack, info);
		}

		/*! NIFLIB_HIDDEN function.  For internal use only. */
		internal override List<NiObject> GetRefs()
		{
			var refs = base.GetRefs();
			return refs;
		}

		/*! NIFLIB_HIDDEN function.  For internal use only. */
		internal override List<NiObject> GetPtrs()
		{
			var ptrs = base.GetPtrs();
			return ptrs;
		}
	}
}
