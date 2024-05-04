using Mirror;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;
    private const string NAME_KEY = "PlayerName";
    
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _nameText;
    
    [SyncVar] private string _name;
    public string Name => _name; 
    
    
    private void Start()
    {
        _name = "Dima";
        _view.SetActive(false);
        
        if (!isOwned)
        {
            //_nameText.text = _name;
        }
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        
        if (PlayerPrefs.HasKey(NAME_KEY))
        {
            var newName = PlayerPrefs.GetString(NAME_KEY, string.Empty);
            SetPlayerNameOnServer(newName);
            //_nameText.text = newName;
        }
        
        if (isLocalPlayer)
        {
            LocalPlayer = this;
        }
    }

    public void SetPlayerName(string newName)
    {
        if (!isOwned) return;
        PlayerPrefs.SetString(NAME_KEY, newName);
        _nameText.text = newName;
        SetPlayerNameOnServer(newName);
    }

    public void SetViewEnable()
    {
        _view.SetActive(true);
    }
    
    public void CreateMatch(string levelName)
    {
        if (!isOwned) return;
        CommandCreateMatch(levelName);
        _view.SetActive(true);
    }
    
    public void JoinMatch(string matchName)
    {
        if (!isOwned) return;
        CommandConnectToMatch(matchName);
        _view.SetActive(true);
    }
    
    [Command]
    private void CommandCreateMatch(string levelName)
    {
        StartCoroutine(NetworkSceneManager.Instance.ServerCreateSubScene(this, levelName));
    }

    [Command]
    private void CommandConnectToMatch(string matchName)
    {
        NetworkSceneManager.Instance.ServerConnectPlayerToMatch(this, matchName);
    }

    [Command]
    private void SetPlayerNameOnServer(string newName)
    {
        _name = newName;
    }
}