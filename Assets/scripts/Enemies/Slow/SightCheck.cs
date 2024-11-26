using UnityEngine;

public static class SightCheck {
    public static bool IsInSight(Transform agent, Vector3 position, float dist = 10f, float cone = 90f) {
        cone = Mathf.Clamp(cone, 0, 360);

        // Distance check
        float agentToPosDist = (position - agent.position).sqrMagnitude;
        bool isInDist = agentToPosDist <= dist * dist;

        // Line of sight check
        Vector3 agentToPosDir = (position - agent.position).normalized;
        float lookness = Vector3.Dot(agent.forward, agentToPosDir);
        float positiveRegion = 1 - 2 * (cone/360);
        bool isInLOS = lookness >= positiveRegion;
        
        return isInDist && isInLOS;
    }
}