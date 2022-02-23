using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField]
    private RewardView _rewardView;

    private RewardController _controller;
    private SaveRepository _saveRepository;


    void Start()
    {
        _saveRepository = new SaveRepository();
        _saveRepository.Initialization();
        _controller = new RewardController(_rewardView, _saveRepository);
    }
}
