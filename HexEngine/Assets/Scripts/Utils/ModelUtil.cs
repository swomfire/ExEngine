﻿using System;

public class ModelUtil
{

    public static float GetModelFullPropertyByFunction(Model model, Func<Model, float> getPropertyValueFunction)
    {
        float propertyValue = getPropertyValueFunction(model);
        if (model == null || model.MountPoints == null)
        {
            return propertyValue;
        }
        foreach (MountPoint mountPoint in model.MountPoints)
        {
            propertyValue += GetModelFullPropertyByFunction(mountPoint.MountedModel, getPropertyValueFunction);
        }
        return propertyValue;
    }

    public static float GetModelFullPropertyByFunctionWithInputObjects(Model model, Func<Model, object[], float> getPropertyValueFunction, object[] inputObjects)
    {
        float propertyValue = getPropertyValueFunction(model, inputObjects);
        if (model == null || model.MountPoints == null)
        {
            return propertyValue;
        }
        foreach (MountPoint mountPoint in model.MountPoints)
        {
            propertyValue += GetModelFullPropertyByFunctionWithInputObjects(mountPoint.MountedModel, getPropertyValueFunction, inputObjects);
        }
        return propertyValue;
    }

    public static void ProcessAllModelByFunctionWithInputObjects(Model model, Action<Model, object[]> processFunction, object[] inputObjects)
    {
        processFunction(model, inputObjects);
        if (model == null || model.MountPoints == null)
        {
            return;
        }
        foreach (MountPoint mountPoint in model.MountPoints)
        {
            ProcessAllModelByFunctionWithInputObjects(mountPoint.MountedModel, processFunction, inputObjects);
        }
    }

    public static bool IsModelMountedTo(Model sourceModel, Model mountedModel)
    {
        if (sourceModel == null || mountedModel == null)
        {
            return false;
        }

        MountPoint mountPoint = sourceModel.CommonPropertySet.MountedTo;
        if (mountPoint == null)
        {
            return false;
        }
        while (mountPoint != null)
        {
            if (mountPoint.SourceModel.GetHashCode() == mountedModel.GetHashCode())
            {
                return true;
            }
            mountPoint = mountPoint.SourceModel.CommonPropertySet.MountedTo;
        }
        return false;
    }
}
