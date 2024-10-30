using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject proyectilePrefab;
    [SerializeField] private float launchModifier;
    [SerializeField] private Transform launchPoint;

    [SerializeField] private GameObject point;
    private GameObject[] pointsList;
    [SerializeField] private int pointsCount;
    [SerializeField] private float spaceBetween;

    private Vector2 direction;

    private void Start() {
        pointsList = new GameObject[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            pointsList[i] = Instantiate(point, launchPoint.position, Quaternion.identity);
        }
    }

    private void Update() {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 launchePosition = transform.position;
        Debug.Log(mousePosition);


        direction = mousePosition - launchePosition;
        

        transform.right = direction;

        if(Input.GetMouseButtonDown(0)){
            Shoot();
        }

        for (int i = 0; i < pointsCount; i++)
        {
            pointsList[i].transform.position = CurrentPosition(i * spaceBetween);
        }
    }

    private void Shoot(){
        GameObject proyectile = Instantiate(proyectilePrefab, launchPoint.position, Quaternion.identity);
        proyectile.GetComponent<Rigidbody2D>().velocity = transform.right * launchModifier;
    }

    private Vector2 CurrentPosition(float t){
        return (Vector2)launchPoint.position + (direction.normalized * launchModifier * t) + (Vector2)(0.5f * Physics.gravity * (t * t));
    }
}
