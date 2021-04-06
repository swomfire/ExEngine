﻿using System;
using UnityEngine;

public class DamageUtil
{
    public static void DamageModel(Model effectedModel, Model sourceModel, float impactValue)
    {
        int rawDamageValue = CombatPropertySetUtil.GetFullRawDamage(sourceModel, impactValue);
        int trueDamageValue = CombatPropertySetUtil.GetFullTrueDamage(sourceModel, impactValue);

        int initialHp = effectedModel.CommonPropertySet.HpStorage.Current;

        ModelUtil.ProcessAllModelByFunctionWithInputObjects(effectedModel, DamageModelWithRawValue, new object[] { rawDamageValue });
        ModelUtil.ProcessAllModelByFunctionWithInputObjects(effectedModel, DamageModelWithTrueValue, new object[] { trueDamageValue });

        Debug.Log(effectedModel.CommonPropertySet.MountType + ":" + initialHp + "->" + effectedModel.CommonPropertySet.HpStorage.Current + ", impactValue:" + impactValue + ", " + rawDamageValue + ", " + trueDamageValue);
    }

    private static void DamageModelWithRawValue(Model effectedModel, object[] inputObjects)
    {
        if (effectedModel == null)
        {
            return;
        }
        int rawDamageValue = (int)inputObjects[0];
        int damageTaken = (int)Math.Ceiling(GetDamageValueAfterAbsorbtion(effectedModel, rawDamageValue) * CombatPropertySetUtil.GetArmorNullifier(effectedModel));
        effectedModel.CommonPropertySet.HpStorage.Fill(damageTaken);
    }

    private static void DamageModelWithTrueValue(Model effectedModel, object[] inputObjects)
    {
        if (effectedModel == null)
        {
            return;
        }
        int trueDamageValue = (int)inputObjects[0];
        int damageTaken = GetDamageValueAfterAbsorbtion(effectedModel, trueDamageValue);
        effectedModel.CommonPropertySet.HpStorage.Fill(damageTaken);
    }

    private static int GetDamageValueAfterAbsorbtion(Model effectedModel, int damageValue)
    {
        if (effectedModel == null)
        {
            return 0;
        }
        int totalArmorAbsobtion = CombatPropertySetUtil.GetFullArmorAbsorbtion(effectedModel);
        if (totalArmorAbsobtion == 0)
        {
            return 0;
        }
        return (int)Math.Ceiling(damageValue * CombatPropertySetUtil.GetArmorAbsorbtion(effectedModel) / totalArmorAbsobtion);
    }
}
