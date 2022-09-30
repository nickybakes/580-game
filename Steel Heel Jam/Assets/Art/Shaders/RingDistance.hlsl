#ifndef RINGDISTANCE_INCLUDED
#define RINGDISTANCE_INCLUDED

uniform float ringRadius = 15;
uniform float2 ringPosition = float2(0, 0);

float GetRadius_float(out float Out) {
    Out = ringRadius;
    return ringRadius;
}

float2 GetPosition_float(out float2 Out) {
    Out = ringPosition;
    return ringPosition;
}


// float GetWorldDistance(float2 worldPosition, out float distance){
//     distance = 0;



//     return distance;
// }


#endif