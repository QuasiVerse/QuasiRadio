using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Nethereum.KeyStore;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Hex.HexTypes;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using ZXing;
using ZXing.QrCode;
using System.Linq;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class Radio : MonoBehaviour {

    //Radio
    
    private decimal RadioAccountQuasiBalance;
    private float RadioBalanceQuasi;
    
    private string radioQuasiKey;
        
    public List<float> radionewfilelist;
    public List<string> radionewfilenamelist;
    public List<string> radionewfileartistlist;
    private Dictionary<BigInteger, RadioNewFileClass> radionewfiledict;
   
    public List<float> radionewtokenlist;
    public List<string> radionewtokennamelist;
    public List<string> radionewtokenartistlist;
    private Dictionary<BigInteger, RadioNewTokenClass> radionewtokendict;
    
    private string radiomessage;
    
    private Controller radioyesbutton;
    private Controller radionobutton;
    private Controller radioopenbutton;
    private Controller radioclosebutton;
    private Controller radiomenubutton;
    private Controller radioplaydownloadsbutton;
    private Controller radioplaystreamsbutton;
    private Controller radiodepositquasibutton;
    private Controller radiowithdrawquasibutton;
    private Controller radiodownloadfilebutton;
    private Controller radiorenewstreambutton;
    private Controller radioplaydownloadbutton;
    private Controller radiostopdownloadbutton;
    private Controller radioplaystreambutton;
    private Controller radiostopstreambutton;
    private Controller radiosearchdisplaystreambuttonprevious;
    private Controller radiosearchdisplaystreambuttonnext;
    private Controller radiosearchdisplaydownloadbuttonprevious;
    private Controller radiosearchdisplaydownloadbuttonnext;
    private Controller radiosearchdisplaydownloadbuttonone;
    private Controller radiosearchdisplaydownloadbuttontwo;
    private Controller radiosearchdisplaydownloadbuttonthree;
    private Controller radiosearchdisplaydownloadbuttonfour;
    private Controller radiosearchdisplaydownloadbuttonfive;
    private Controller radiosearchdisplaystreambuttonone;
    private Controller radiosearchdisplaystreambuttontwo;
    private Controller radiosearchdisplaystreambuttonthree;
    private Controller radiosearchdisplaystreambuttonfour;
    private Controller radiosearchdisplaystreambuttonfive;
    private Slider radiosliderdownloadbutton;
    private Slider radiosliderstreambutton;
    private InputField radiosearchdownloadbutton;
    private InputField radiosearchstreambutton;
    private InputField radiodepositamount;
    private InputField radiowithdrawamount;
    private InputField radiopasswordinput;
    private Text radiodepositamounttext;
    private Text radiowithdrawamounttext;
    private Text radiosearchdisplaydownloadbuttononetext;
    private Text radiosearchdisplaydownloadbuttontwotext;
    private Text radiosearchdisplaydownloadbuttonthreetext;
    private Text radiosearchdisplaydownloadbuttonfourtext;
    private Text radiosearchdisplaydownloadbuttonfivetext;
    private Text radiosearchdisplaystreambuttononetext;
    private Text radiosearchdisplaystreambuttontwotext;
    private Text radiosearchdisplaystreambuttonthreetext;
    private Text radiosearchdisplaystreambuttonfourtext;
    private Text radiosearchdisplaystreambuttonfivetext;
    private Text radioquasibalance;

    public AudioSource RadioSource;
    public List<AudioClip> radioclips = new List<AudioClip>();
  
    private FileInfo[] RadioDownloadFiles;
    private List<string> RadioValidExtensions = new List<string> { ".mp3" };
    private string AbsoluteRadioPath;

    private string radioURL;
    private string radioTokenURI;
    private List<string> radiosearchdownloadresult;
    private List<string> radiosearchstreamresult;
    
    private bool radioaccess;
    private bool quasiradio;
    private bool radioplaydownloads;
    private bool radioplaystreams;
    private bool radiodepositquasi;
    private bool radiowithdrawquasi;
    private bool radioconfirmdepositquasi;
    private bool radioconfirmwithdrawquasi;
    private bool radiodownloadfile;
    private bool radiorenewstream;

    private string oldradiosearchdownloadbuttontext;
    private string oldradiosearchstreambuttontext;
    
    private int radiosearchindex;
    private int radiodownloadnumber;
    private int radiostreamnumber;
    
    private float radiodownloadslider;
    private float radiostreamslider;
    
    private float oldradiodownloadslider;
    private float oldradiostreamslider;
    
    private float updatetimer = 0f;
    private float buttontimer = 0f;
    
    public InvPauseGame ipg;
    public NetworkManager nm;
    
    //Screen
    
    private float screentimer;
    private bool screenplaying;
    private bool screenmenuactive;
    private RawImage screenimage;
    public Texture RadioScreenImage;
    public MovieTexture TelevisionScreenImage;
    
    //Address
    
    private Account account;
    private Address addressfile;
    
    private string QZIErc721Address = "";
    
    private string QZIAddress = "";
    private string CTYAddress = "";
    
    private string QZIETHExchangeAddress = "";
    private string QZICTYExchangeAddress = "";
    
    private string ERC721ExchangeAddress = "";
    
    private string buyItemAddress = "";
    
    private string advertisementAddress = "";
    
    private string radioAddress = "";
    
    private string televisionAddress = "";
    
    private string workAddress = "";
        
    private bool multiexchange;
    
    private string erc20coinabbr = "";
    private string erc20coinname = "";
    private string erc20TokenAddress = "";
    private string secondarycoinname = "";
    
    private string erc721coinabbr = "";
    private string erc721coinname = "";
    private string erc721TokenAddress = "";
    
    private int erc20coinnum = 1;
    private int erc721coinnum = 1;
    
    private string accountAddress;
    
    private string password = "";
    
    private bool radioinit;
    
    private string _url = "https://ropsten.infura.io/V1bTAz1YhoWi54jDHFyK ";
    
    private string gasallowed = "300000";
    private string gasprice = "10000000000";
    
    private string accountEncryptedJson = "";
        
	// Use this for initialization
	void Start () {
        
        //Declarations
        
        radioyesbutton = GameObject.Find("Radio").transform.FindChild("Yes").GetComponent<Controller>();
        radionobutton = GameObject.Find("Radio").transform.FindChild("No").GetComponent<Controller>();
        radioopenbutton = GameObject.Find("Menu").transform.FindChild("Radio Button").GetComponent<Controller>();
        radioclosebutton = GameObject.Find("Radio").transform.FindChild("Close").GetComponent<Controller>();
        radiomenubutton = GameObject.Find("Radio").transform.FindChild("Menu Button").GetComponent<Controller>();
        radioplaydownloadsbutton = GameObject.Find("Radio").transform.FindChild("Play Downloads").GetComponent<Controller>();
        radioplaystreamsbutton = GameObject.Find("Radio").transform.FindChild("Play Streams").GetComponent<Controller>();
        radiodepositquasibutton = GameObject.Find("Radio").transform.FindChild("Deposit Quasi").GetComponent<Controller>();
        radiowithdrawquasibutton = GameObject.Find("Radio").transform.FindChild("Withdraw Quasi").GetComponent<Controller>();
        radiodownloadfilebutton = GameObject.Find("Radio").transform.FindChild("Download File").GetComponent<Controller>();
        radiorenewstreambutton = GameObject.Find("Radio").transform.FindChild("Renew Stream").GetComponent<Controller>();
        radioplaydownloadbutton = GameObject.Find("Radio").transform.FindChild("Play Download").GetComponent<Controller>();
        radiostopdownloadbutton = GameObject.Find("Radio").transform.FindChild("Stop Download").GetComponent<Controller>();
        radioplaystreambutton = GameObject.Find("Radio").transform.FindChild("Play Stream").GetComponent<Controller>();
        radiostopstreambutton = GameObject.Find("Radio").transform.FindChild("Stop Stream").GetComponent<Controller>();
        
        radiosearchdisplaystreambuttonprevious = GameObject.Find("Radio").transform.FindChild("Display Stream Previous").GetComponent<Controller>();
        radiosearchdisplaystreambuttonnext = GameObject.Find("Radio").transform.FindChild("Display Stream Next").GetComponent<Controller>();
        radiosearchdisplaydownloadbuttonprevious = GameObject.Find("Radio").transform.FindChild("Display Download Previous").GetComponent<Controller>();
        radiosearchdisplaydownloadbuttonnext = GameObject.Find("Radio").transform.FindChild("Display Download Next").GetComponent<Controller>();
        radiosearchdisplaydownloadbuttonone = GameObject.Find("Radio").transform.FindChild("Display Download One").GetComponent<Controller>();
        radiosearchdisplaydownloadbuttontwo = GameObject.Find("Radio").transform.FindChild("Display Download Two").GetComponent<Controller>();
        radiosearchdisplaydownloadbuttonthree = GameObject.Find("Radio").transform.FindChild("Display Download Three").GetComponent<Controller>();
        radiosearchdisplaydownloadbuttonfour = GameObject.Find("Radio").transform.FindChild("Display Download Four").GetComponent<Controller>();
        radiosearchdisplaydownloadbuttonfive = GameObject.Find("Radio").transform.FindChild("Display Download Five").GetComponent<Controller>();
        radiosearchdisplaystreambuttonone = GameObject.Find("Radio").transform.FindChild("Display Stream One").GetComponent<Controller>();
        radiosearchdisplaystreambuttontwo = GameObject.Find("Radio").transform.FindChild("Display Stream Two").GetComponent<Controller>();
        radiosearchdisplaystreambuttonthree = GameObject.Find("Radio").transform.FindChild("Display Stream Three").GetComponent<Controller>();
        radiosearchdisplaystreambuttonfour = GameObject.Find("Radio").transform.FindChild("Display Stream Four").GetComponent<Controller>();
        radiosearchdisplaystreambuttonfive = GameObject.Find("Radio").transform.FindChild("Display Stream Five").GetComponent<Controller>();
        
        radiosliderdownloadbutton = GameObject.Find("Radio").transform.FindChild("Slider Download").GetComponent<Slider>();
        radiosliderstreambutton = GameObject.Find("Radio").transform.FindChild("Slider Stream").GetComponent<Slider>();
        radiosearchdownloadbutton = GameObject.Find("Radio").transform.FindChild("Search Download").GetComponent<InputField>();
        radiosearchstreambutton = GameObject.Find("Radio").transform.FindChild("Search Stream").GetComponent<InputField>();
        radiodepositamount = GameObject.Find("Radio").transform.FindChild("Deposit Amount").GetComponent<InputField>();
        radiowithdrawamount = GameObject.Find("Radio").transform.FindChild("Withdraw Amount").GetComponent<InputField>();
        radiopasswordinput = GameObject.Find("Radio").transform.FindChild("Password").GetComponent<InputField>();
        
        radiodepositamounttext = GameObject.Find("Radio").transform.FindChild("Deposit Amount Text").GetComponent<Text>();
        radiowithdrawamounttext = GameObject.Find("Radio").transform.FindChild("Withdraw Amount Text").GetComponent<Text>();
        radiosearchdisplaydownloadbuttononetext = GameObject.Find("Radio").transform.FindChild("Display Download One Text").GetComponent<Text>();
        radiosearchdisplaydownloadbuttontwotext = GameObject.Find("Radio").transform.FindChild("Display Download Two Text").GetComponent<Text>();
        radiosearchdisplaydownloadbuttonthreetext = GameObject.Find("Radio").transform.FindChild("Display Download Three Text").GetComponent<Text>();
        radiosearchdisplaydownloadbuttonfourtext = GameObject.Find("Radio").transform.FindChild("Display Download Four Text").GetComponent<Text>();
        radiosearchdisplaydownloadbuttonfivetext = GameObject.Find("Radio").transform.FindChild("Display Download Five Text").GetComponent<Text>();
        radiosearchdisplaystreambuttononetext = GameObject.Find("Radio").transform.FindChild("Display Stream One Text").GetComponent<Text>();
        radiosearchdisplaystreambuttontwotext = GameObject.Find("Radio").transform.FindChild("Display Stream Two Text").GetComponent<Text>();
        radiosearchdisplaystreambuttonthreetext = GameObject.Find("Radio").transform.FindChild("Display Stream Three Text").GetComponent<Text>();
        radiosearchdisplaystreambuttonfourtext = GameObject.Find("Radio").transform.FindChild("Display Stream Four Text").GetComponent<Text>();
        radiosearchdisplaystreambuttonfivetext = GameObject.Find("Radio").transform.FindChild("Display Stream Five Text").GetComponent<Text>();
        radioquasibalance = GameObject.Find("Radio").transform.FindChild("Quasi Balance").GetComponent<Text>();
        radiosearchdownloadresult = new List<String>();
        radiosearchstreamresult = new List<String>();
        
        AbsoluteRadioPath = Application.persistentDataPath + "/Radio/";
        if (Application.isEditor){
            AbsoluteRadioPath = "Assets/Radio/";
        }
 
        RadioSource = GetComponent<AudioSource>(); 
        RadioDownloadLoad();
        
        RadioValidExtensions = new List<string> { ".mp3" };
        
        //Screen
        
        screenimage = GameObject.Find("Radio").transform.FindChild("RawImage").GetComponent<RawImage>();
        
        //Address

        account = this.transform.GetComponent<Account>();
        addressfile = this.transform.GetComponent<Address>();
        
        string[] tempaddressarray = addressfile.AddressReturn();
        QZIErc721Address = tempaddressarray[0];
        QZIAddress = tempaddressarray[1];
        CTYAddress = tempaddressarray[2];
        QZIETHExchangeAddress = tempaddressarray[3];
        QZICTYExchangeAddress = tempaddressarray[4];
        ERC721ExchangeAddress = tempaddressarray[5];
        buyItemAddress = tempaddressarray[6];
        advertisementAddress = tempaddressarray[7];
        radioAddress = tempaddressarray[8];
        televisionAddress = tempaddressarray[9];
        workAddress = tempaddressarray[10];
        
        tempaddressarray = null;
        
        nm = GameObject.Find("NM").transform.GetComponent<NetworkManager>();
        
        accountEncryptedJson = PlayerPrefs.GetString("PK");
        
	}
	
    void FixedUpdate () {
        
        updatetimer -= .1f;
        buttontimer += .1f;
        
        //Data
        
        if(updatetimer > 40f && accountAddress != ""){
            if(radioaccess == true){
                RadioGetNewFileEventsRequest();
                RadioGetNewTokenEventsRequest();
                RadioGetBalanceOfQuasi(accountAddress);
            }
            updatetimer = 0f;
        }
        
        erc20coinnum = account.erc20coinnum;
        erc721coinnum = account.erc721coinnum;
        
        if(nm.myPlayerGo != null && radioinit == false){
            ipg = nm.myPlayerGo.transform.FindChild("Inventory").GetComponent<InvPauseGame>();
            radioinit = true;
        }
    }
    
	// Update is called once per frame
	void Update () {
        
        //Interface
        
        if(radioopenbutton.InputDirection != Vector3.zero && buttontimer > 1f && account.walletaccess == true){
            radioopenbutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            radioplaydownloads = false;
            radioplaystreams = false;
            radiodepositquasi = false;
            radiowithdrawquasi = false;
            radiodownloadfile = false;
            radiorenewstream = false;
            quasiradio = true; 
            ipg.radioon = true;
            ipg.PauseGame (true, false);
            radiosearchdisplaydownloadbuttonone.gameObject.active = false;
            radiosearchdisplaydownloadbuttontwo.gameObject.active = false;
            radiosearchdisplaydownloadbuttonthree.gameObject.active = false;
            radiosearchdisplaydownloadbuttonfour.gameObject.active = false;
            radiosearchdisplaydownloadbuttonfive.gameObject.active = false;
            radiosearchdisplaystreambuttonone.gameObject.active = false;
            radiosearchdisplaystreambuttontwo.gameObject.active = false;
            radiosearchdisplaystreambuttonthree.gameObject.active = false;
            radiosearchdisplaystreambuttonfour.gameObject.active = false;
            radiosearchdisplaystreambuttonfive.gameObject.active = false;

        }
        if(radioclosebutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radioclosebutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            ipg.walleton = true;
            ipg.PauseGame (true, true);
            radioplaydownloads = false;
            radioplaystreams = false;
            radiodepositquasi = false;
            radiowithdrawquasi = false;
            radiodownloadfile = false;
            radiorenewstream = false;
            quasiradio = false;
            screenplaying = false;
            ipg.radioon = false;
            ipg.PauseGame (false, false);
        }
        if(radiomenubutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radiomenubutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            ipg.radioon = true;
            ipg.PauseGame (true, true);
            radioplaydownloads = false;
            radioplaystreams = false;
            radiodepositquasi = false;
            radiowithdrawquasi = false;
            radiodownloadfile = false;
            radiorenewstream = false;
            screenplaying = false;
        }
        if(radioplaydownloadsbutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radioplaydownloadsbutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            radioplaydownloads = true;
        }
        if(radioplaystreamsbutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radioplaystreamsbutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            radioplaystreams = true;
        }
        if(radiodepositquasibutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radiodepositquasibutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            radiodepositquasi = true;
        }
        if(radiowithdrawquasibutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radiowithdrawquasibutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            radiowithdrawquasi = true;
        }
        if(radiodownloadfilebutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radiodownloadfilebutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            radiodownloadfile = true;
        }
        if(radiorenewstreambutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radiorenewstreambutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            radiorenewstream = true;
        }
        
        
        if(radioplaydownloadbutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radioplaydownloadbutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            StartCoroutine(RadioPlayDownload());
            radioplaydownloads = true;
        }
        if(radiostopdownloadbutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radiostopdownloadbutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            RadioStopDownload();
        }
        if(radioplaystreambutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radioplaystreambutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            StartCoroutine(RadioPlayStream());
        }
        if(radiostopdownloadbutton.InputDirection != Vector3.zero && buttontimer > 1f){
            radiostopdownloadbutton.InputDirection = Vector3.zero;
            buttontimer = 0f;
            RadioStopStream();
        }
        
        if(radiosearchdownloadbutton.text != oldradiosearchdownloadbuttontext){
            oldradiosearchdownloadbuttontext = radiosearchdownloadbutton.text;
            SearchRadioDownload(oldradiosearchdownloadbuttontext);
        }
        
        if(radiosearchstreambutton.text != oldradiosearchstreambuttontext){
            oldradiosearchstreambuttontext = radiosearchstreambutton.text;
            SearchRadioStream(oldradiosearchstreambuttontext);
        }
        if(radiosearchdisplaydownloadbuttonone.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaydownloadbuttonone.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchdownloadresult[0 + (5 * radiosearchindex)];
            for(int s = 0; s < RadioDownloadFiles.Length; s++){
                if(RadioDownloadFiles[s].FullName.Contains(tempstring)){
                    radiodownloadnumber = s;
                }
            }
            tempstring = "";
        }
        if(radiosearchdisplaydownloadbuttontwo.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaydownloadbuttontwo.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchdownloadresult[1 + (5 * radiosearchindex)];
            for(int s = 0; s < RadioDownloadFiles.Length; s++){
                if(RadioDownloadFiles[s].FullName.Contains(tempstring)){
                    radiodownloadnumber = s;
                }
            }
            tempstring = "";
        }
        if(radiosearchdisplaydownloadbuttonthree.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaydownloadbuttonthree.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchdownloadresult[2 + (5 * radiosearchindex)];
            for(int s = 0; s < RadioDownloadFiles.Length; s++){
                if(RadioDownloadFiles[s].FullName.Contains(tempstring)){
                    radiodownloadnumber = s;
                }
            }
            tempstring = "";
        }
        if(radiosearchdisplaydownloadbuttonfour.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaydownloadbuttonfour.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchdownloadresult[3 + (5 * radiosearchindex)];
            for(int s = 0; s < RadioDownloadFiles.Length; s++){
                if(RadioDownloadFiles[s].FullName.Contains(tempstring)){
                    radiodownloadnumber = s;
                }
            }
            tempstring = "";
        }
        if(radiosearchdisplaydownloadbuttonfive.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaydownloadbuttonfive.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchdownloadresult[4 + (5 * radiosearchindex)];
            for(int s = 0; s < RadioDownloadFiles.Length; s++){
                if(RadioDownloadFiles[s].FullName.Contains(tempstring)){
                    radiodownloadnumber = s;
                }
            }
            tempstring = "";
        }
        
        if(radiosearchdisplaystreambuttonone.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaystreambuttonone.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchstreamresult[0 + (5 * radiosearchindex)];
            for(int s = 0; s < radionewfilenamelist.Count; s++){
                if(radionewfilenamelist[s].Contains(tempstring)){
                    radiostreamnumber = s;
                }
            }
            tempstring = "";
        }
        if(radiosearchdisplaystreambuttontwo.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaystreambuttontwo.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchstreamresult[1 + (5 * radiosearchindex)];
            for(int s = 0; s < radionewfilenamelist.Count; s++){
                if(radionewfilenamelist[s].Contains(tempstring)){
                    radiostreamnumber = s;
                }
            }
            tempstring = "";
        }
        if(radiosearchdisplaystreambuttonthree.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaystreambuttonthree.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchstreamresult[2 + (5 * radiosearchindex)];
            for(int s = 0; s < radionewfilenamelist.Count; s++){
                if(radionewfilenamelist[s].Contains(tempstring)){
                    radiostreamnumber = s;
                }
            }
            tempstring = "";
        }
        if(radiosearchdisplaystreambuttonfour.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaystreambuttonfour.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchstreamresult[3 + (5 * radiosearchindex)];
            for(int s = 0; s < radionewfilenamelist.Count; s++){
                if(radionewfilenamelist[s].Contains(tempstring)){
                    radiostreamnumber = s;
                }
            }
            tempstring = "";
        }
        if(radiosearchdisplaystreambuttonfive.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaystreambuttonfive.InputDirection = Vector3.zero;
            buttontimer = 0f;
            string tempstring = radiosearchstreamresult[4 + (5 * radiosearchindex)];
            for(int s = 0; s < radionewfilenamelist.Count; s++){
                if(radionewfilenamelist[s].Contains(tempstring)){
                    radiostreamnumber = s;
                }
            }
            tempstring = "";
        }
        
        if(radiosearchdisplaydownloadbuttonprevious.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaydownloadbuttonprevious.InputDirection = Vector3.zero;
            buttontimer = 0f;
            if(radiosearchindex > 0){
                radiosearchindex -= 1;
            }
        }
        if(radiosearchdisplaydownloadbuttonnext.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaydownloadbuttonnext.InputDirection = Vector3.zero;
            buttontimer = 0f;
            if(radiosearchindex < radiosearchdownloadresult.Count/5){
                radiosearchindex += 1;
            }
        }
        if(radiosearchdisplaystreambuttonprevious.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaystreambuttonprevious.InputDirection = Vector3.zero;
            buttontimer = 0f;
            if(radiosearchindex > 0){
                radiosearchindex -= 1;
            }
        }
        if(radiosearchdisplaystreambuttonnext.InputDirection != Vector3.zero && buttontimer > 1f){
            radiosearchdisplaystreambuttonnext.InputDirection = Vector3.zero;
            buttontimer = 0f;
            if(radiosearchindex < radiosearchstreamresult.Count/5){
                radiosearchindex += 1;
            }
        }
        
        if(quasiradio == true){
            erc20coinnum = 1;
        }
        
        if(quasiradio == true && radiodepositquasi == false && radiowithdrawquasi == false && radioconfirmdepositquasi == false && radioconfirmwithdrawquasi == false && radiodownloadfile == false && radiorenewstream == false && radioplaydownloads == false && radioplaystreams == false){
            radioclosebutton.gameObject.active = true;
            radiomenubutton.gameObject.active = true;
            radioplaydownloadsbutton.gameObject.active = true;
            radioplaystreamsbutton.gameObject.active = true;
            radiodepositquasibutton.gameObject.active = true;
            radiowithdrawquasibutton.gameObject.active = true;
            radiodownloadfilebutton.gameObject.active = true;
            radiorenewstreambutton.gameObject.active = true;
        }
        if(radiodepositquasi == true || radiowithdrawquasi == true || radioconfirmdepositquasi == true || radioconfirmwithdrawquasi == true || radiodownloadfile == true || radiorenewstream == true || radioplaydownloads == true || radioplaystreams == true){
            radioclosebutton.gameObject.active = false;
            radiomenubutton.gameObject.active = false;
            radioplaydownloadsbutton.gameObject.active = false;
            radioplaystreamsbutton.gameObject.active = false;
            radiodepositquasibutton.gameObject.active = false;
            radiowithdrawquasibutton.gameObject.active = false;
            radiodownloadfilebutton.gameObject.active = false;
            radiorenewstreambutton.gameObject.active = false;
        }
        
        if(radiodepositquasi == true){
            radiodepositamount.gameObject.active = true;
            if(radionobutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radionobutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                radiodepositquasi = false;
            }
            if(radioyesbutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radioyesbutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                if(password != ""){
                    radiodepositamounttext.text = radiodepositamount.text;
                    radiodepositquasi = false;
                    radioconfirmdepositquasi = true;
                }
                if(password == ""){
                    radiomessage = "Please Enter Password";
                }
            }
        }
        if(radiodepositquasi == false){
            radiodepositamount.gameObject.active = false;
        }
        if(radioconfirmdepositquasi == true){
            radiodepositamounttext.gameObject.active = true;
            if(radionobutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radionobutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                radioconfirmdepositquasi = false;
            }
            if(radioyesbutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radioyesbutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                if(password != ""){
                    RadioDepositQuasi(float.Parse(radiodepositamounttext.text), password);
                    radioconfirmdepositquasi = false;
                }
                if(password == ""){
                    radiomessage = "Please Enter Password";
                }
            }
        }
        if(radioconfirmdepositquasi == false){
            radiodepositamounttext.gameObject.active = false;
        }
        if(radiowithdrawquasi == true){
            radiowithdrawamount.gameObject.active = true;
            if(radionobutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radionobutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                radiowithdrawquasi = false;
            }
            if(radioyesbutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radioyesbutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                if(password != ""){
                    radiowithdrawamounttext.text = radiowithdrawamount.text;
                    radiowithdrawquasi = false;
                    radioconfirmwithdrawquasi = true;
                }
                if(password == ""){
                    radiomessage = "Please Enter Password";
                }
            }
        }
        if(radioconfirmwithdrawquasi == false){
            radiowithdrawamount.gameObject.active = false;
        }
        if(radioconfirmwithdrawquasi == true){
            radiowithdrawamounttext.gameObject.active = true;
            if(radionobutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radionobutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                radioconfirmwithdrawquasi = false;
            }
            if(radioyesbutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radioyesbutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                if(password != ""){
                    RadioWithdrawQuasi(float.Parse(radiowithdrawamounttext.text), password);
                    radioconfirmwithdrawquasi = false;
                }
                if(password == ""){
                    radiomessage = "Please Enter Password";
                }
            }
        }
        if(radiowithdrawquasi == false){
            radiowithdrawamounttext.gameObject.active = false;
        }
        if(radiodownloadfile == true){
            if(radionobutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radionobutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                radiodownloadfile = false;
            }
            if(radioyesbutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radioyesbutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                if(password != ""){
                    RadioCreateCopy(radiostreamnumber.ToString("0000000000000000000000000000000000000000000000000000000000000000"), password);
                    radiodownloadfile = false;
                }
                if(password == ""){
                    radiomessage = "Please Enter Password";
                }
            }
        }
        if(radiorenewstream == true){
            if(password == ""){
                radiomessage = "Please Enter Password";
            }
            if(password != ""){
                radiomessage = "Renew Stream";
            }
            if(radionobutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radionobutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                radiodownloadfile = false;
            }
            if(radioyesbutton.InputDirection != Vector3.zero && buttontimer > 1f){
                radioyesbutton.InputDirection = Vector3.zero;
                buttontimer = 0f;
                if(password != ""){
                    RadioCreateStreamRenewal(password);
                    radiodownloadfile = false;
                }
            }
        }
        if(radioplaydownloads == true && screenplaying == false){
            radiosearchdownloadbutton.gameObject.active = true;
            radiosearchdisplaydownloadbuttonprevious.gameObject.active = true;
            radiosearchdisplaydownloadbuttonnext.gameObject.active = true;
        }
        if(radioplaydownloads == false){
            radioplaydownloadbutton.gameObject.active = false;
            radiostopdownloadbutton.gameObject.active = false;
            radiosliderdownloadbutton.gameObject.active = false;
            radiosearchdownloadbutton.gameObject.active = false;
            radiosearchdisplaydownloadbuttonprevious.gameObject.active = false;
            radiosearchdisplaydownloadbuttonnext.gameObject.active = false;
        }
        if(radioplaystreams == true && screenplaying == false){
            radioplaystreambutton.gameObject.active = true;
            radiostopstreambutton.gameObject.active = true;
            radiosliderstreambutton.gameObject.active = true;
            radiosearchstreambutton.gameObject.active = true;
            radiosearchdisplaystreambuttonprevious.gameObject.active = true;
            radiosearchdisplaystreambuttonnext.gameObject.active = true;
        }
        if(radioplaystreams == false){
            radioplaystreambutton.gameObject.active = false;
            radiostopstreambutton.gameObject.active = false;
            radiosliderstreambutton.gameObject.active = false;
            radiosearchstreambutton.gameObject.active = false;
            radiosearchdisplaystreambuttonprevious.gameObject.active = false;
            radiosearchdisplaystreambuttonnext.gameObject.active = false;
        }
        if(radiodepositquasi == true || radiowithdrawquasi == true || radioconfirmdepositquasi == true || radioconfirmwithdrawquasi == true || radiodownloadfile == true || radiorenewstream == true){
            radioyesbutton.gameObject.active = true;
            radionobutton.gameObject.active = true;
        }
        if(radiodepositquasi == true && radiowithdrawquasi == true && radioconfirmdepositquasi == true && radioconfirmwithdrawquasi == true && radiodownloadfile == true && radiorenewstream == true){
            radioyesbutton.gameObject.active = false;
            radionobutton.gameObject.active = false;
        }
        
        if(radiodownloadslider > oldradiodownloadslider + 5 || radiodownloadslider < oldradiodownloadslider - 5){
            oldradiodownloadslider = radiodownloadslider;
            RadioSource.time = oldradiodownloadslider;
        }
        
        if(radiostreamslider > oldradiostreamslider + 5 || radiostreamslider < oldradiostreamslider - 5){
            oldradiostreamslider = radiostreamslider;
            RadioSource.time = oldradiostreamslider;
        }
        
        radiodownloadslider = RadioSource.time;
        radiostreamslider = RadioSource.time;
        
        //Screen

        if(screenplaying == true){
            screenimage.gameObject.active = true;
        }
        if(screenplaying == false){
            screenimage.gameObject.active = false;
        }
        if(screenmenuactive == true && screenplaying == true){
            if(quasiradio == true){
                if(radioplaydownloads == true){
                    radioclosebutton.gameObject.active = true;
                    radiomenubutton.gameObject.active = true;
                    radioplaydownloadbutton.gameObject.active = true;
                    radiostopdownloadbutton.gameObject.active = true;
                    radiosliderdownloadbutton.gameObject.active = true;
                }
                if(radioplaydownloads == false){
                    radiosearchdisplaydownloadbuttonprevious.gameObject.active = false;
                    radiosearchdisplaydownloadbuttonnext.gameObject.active = false;
                }
                if(radioplaystreams == true){
                    radioclosebutton.gameObject.active = true;
                    radiomenubutton.gameObject.active = true;
                    radioplaystreambutton.gameObject.active = true;
                    radiostopstreambutton.gameObject.active = true;
                    radiosliderstreambutton.gameObject.active = true;
                }
                if(radioplaystreams == false){
                    radiosearchdisplaystreambuttonprevious.gameObject.active = false;
                    radiosearchdisplaystreambuttonnext.gameObject.active = false;
                }
            }  
        }
        if(screenmenuactive == false && screenplaying == true){   
            if(quasiradio == true){
                if(radioplaydownloads == true){
                    radioclosebutton.gameObject.active = false;
                    radiomenubutton.gameObject.active = false;
                    radioplaydownloadbutton.gameObject.active = false;
                    radiostopdownloadbutton.gameObject.active = false;
                    radiosliderdownloadbutton.gameObject.active = false;
                }
                if(radioplaystreams == true){
                    radioclosebutton.gameObject.active = false;
                    radiomenubutton.gameObject.active = false;
                    radioplaystreambutton.gameObject.active = false;
                    radiostopstreambutton.gameObject.active = false;
                    radiosliderstreambutton.gameObject.active = false;
                }
            }  
        }
        if(quasiradio == true){
            screenimage.texture = RadioScreenImage;
        }
        
        //Address
        
        if(erc20coinnum == 1){
            erc20coinabbr = "QZI";
            erc20coinname = "Quasi";
            erc20TokenAddress = QZIAddress;
            secondarycoinname = "Quasi";
        }
        if(erc20coinnum == 2){
            erc20coinabbr = "ETH";
            erc20coinname = "Ether";
            secondarycoinname = "Ether";
        }
        if(erc20coinnum == 3){
            erc20coinabbr = "CTY";
            erc20coinname = "Country";
            erc20TokenAddress = CTYAddress;
            secondarycoinname = "Country";
        }

        
        if(erc20coinnum == 1000001){
            erc20coinabbr = PlayerPrefs.GetString("ERC20:" + "1" + "ABBR");
            erc20coinname = PlayerPrefs.GetString("ERC20:" + "1" + "NAME");
            erc20TokenAddress = PlayerPrefs.GetString("ERC20:" + "1" + "ADDR");
        }
        if(erc20coinnum == 1000002){
            erc20coinabbr = PlayerPrefs.GetString("ERC20:" + "2" + "ABBR");
            erc20coinname = PlayerPrefs.GetString("ERC20:" + "2" + "NAME");
            erc20TokenAddress = PlayerPrefs.GetString("ERC20:" + "2" + "ADDR");
        }
        if(erc20coinnum == 1000003){
            erc20coinabbr = PlayerPrefs.GetString("ERC20:" + "3" + "ABBR");
            erc20coinname = PlayerPrefs.GetString("ERC20:" + "3" + "NAME");
            erc20TokenAddress = PlayerPrefs.GetString("ERC20:" + "3" + "ADDR");
        }
        if(erc20coinnum == 1000004){
            erc20coinabbr = PlayerPrefs.GetString("ERC20:" + "4" + "ABBR");
            erc20coinname = PlayerPrefs.GetString("ERC20:" + "4" + "NAME");
            erc20TokenAddress = PlayerPrefs.GetString("ERC20:" + "4" + "ADDR");
        }
        if(erc20coinnum == 1000005){
            erc20coinabbr = PlayerPrefs.GetString("ERC20:" + "5" + "ABBR");
            erc20coinname = PlayerPrefs.GetString("ERC20:" + "5" + "NAME");
            erc20TokenAddress = PlayerPrefs.GetString("ERC20:" + "5" + "ADDR");
        }
        if(erc20coinnum == 1000006){
            erc20coinabbr = PlayerPrefs.GetString("ERC20:" + "6" + "ABBR");
            erc20coinname = PlayerPrefs.GetString("ERC20:" + "6" + "NAME");
            erc20TokenAddress = PlayerPrefs.GetString("ERC20:" + "6" + "ADDR");
        }
        if(erc20coinnum == 1000007){
            erc20coinabbr = PlayerPrefs.GetString("ERC20:" + "7" + "ABBR");
            erc20coinname = PlayerPrefs.GetString("ERC20:" + "7" + "NAME");
            erc20TokenAddress = PlayerPrefs.GetString("ERC20:" + "7" + "ADDR");
        }
        if(erc20coinnum == 1000008){
            erc20coinabbr = PlayerPrefs.GetString("ERC20:" + "8" + "ABBR");
            erc20coinname = PlayerPrefs.GetString("ERC20:" + "8" + "NAME");
            erc20TokenAddress = PlayerPrefs.GetString("ERC20:" + "8" + "ADDR");
        }
        
        
        if(erc721coinnum == 1){
            erc721coinabbr = "QZI";
            erc721coinname = "Quasi";
            erc721TokenAddress = QZIErc721Address;
        }
    
        
        if(erc721coinnum == 1000001){
            erc721coinabbr = PlayerPrefs.GetString("ERC721:" + "1" + "ABBR");
            erc721coinname = PlayerPrefs.GetString("ERC721:" + "1" + "NAME");
            erc721TokenAddress = PlayerPrefs.GetString("ERC721:" + "1" + "ADDR");
        }
        if(erc721coinnum == 1000002){
            erc721coinabbr = PlayerPrefs.GetString("ERC721:" + "2" + "ABBR");
            erc721coinname = PlayerPrefs.GetString("ERC721:" + "2" + "NAME");
            erc721TokenAddress = PlayerPrefs.GetString("ERC721:" + "2" + "ADDR");
        }
        if(erc721coinnum == 1000003){
            erc721coinabbr = PlayerPrefs.GetString("ERC721:" + "3" + "ABBR");
            erc721coinname = PlayerPrefs.GetString("ERC721:" + "3" + "NAME");
            erc721TokenAddress = PlayerPrefs.GetString("ERC721:" + "3" + "ADDR");
        }
        if(erc721coinnum == 1000004){
            erc721coinabbr = PlayerPrefs.GetString("ERC721:" + "4" + "ABBR");
            erc721coinname = PlayerPrefs.GetString("ERC721:" + "4" + "NAME");
            erc721TokenAddress = PlayerPrefs.GetString("ERC721:" + "4" + "ADDR");
        }
        if(erc721coinnum == 1000005){
            erc721coinabbr = PlayerPrefs.GetString("ERC721:" + "5" + "ABBR");
            erc721coinname = PlayerPrefs.GetString("ERC721:" + "5" + "NAME");
            erc721TokenAddress = PlayerPrefs.GetString("ERC721:" + "5" + "ADDR");
        }
        if(erc721coinnum == 1000006){
            erc721coinabbr = PlayerPrefs.GetString("ERC721:" + "6" + "ABBR");
            erc721coinname = PlayerPrefs.GetString("ERC721:" + "6" + "NAME");
            erc721TokenAddress = PlayerPrefs.GetString("ERC721:" + "6" + "ADDR");
        }
        if(erc721coinnum == 1000007){
            erc721coinabbr = PlayerPrefs.GetString("ERC721:" + "7" + "ABBR");
            erc721coinname = PlayerPrefs.GetString("ERC721:" + "7" + "NAME");
            erc721TokenAddress = PlayerPrefs.GetString("ERC721:" + "7" + "ADDR");
        }
        if(erc721coinnum == 1000008){
            erc721coinabbr = PlayerPrefs.GetString("ERC721:" + "8" + "ABBR");
            erc721coinname = PlayerPrefs.GetString("ERC721:" + "8" + "NAME");
            erc721TokenAddress = PlayerPrefs.GetString("ERC721:" + "8" + "ADDR");
        }
	}
    
    //Interactions
    
    public void SearchRadioStream(string radiodata){
        radiosearchstreamresult.Clear();
        for(int s = 0; s < radionewfilenamelist.Count; s++){
            string stringdata = "";
            bool found = false;
            if(radionewfilenamelist[s].Contains(radiodata)){
                stringdata = radionewfilelist[s] + " - " + radionewfilenamelist[s] + " - " + radionewfileartistlist[s];
                found = true;
            }
            if(radionewfileartistlist[s].Contains(radiodata) && found == false){
                stringdata = radionewfilelist[s] + " - " + radionewfilenamelist[s] + " - " + radionewfileartistlist[s];
            }
            radiosearchstreamresult.Add(stringdata + ".mp3");
        }
        RadioDisplaySearchedStream();
    }
    
    public void SearchRadioDownload(string radiodata){
        radiosearchdownloadresult.Clear();
        foreach(var s in RadioDownloadFiles){
            if(s.FullName.Contains(radiodata)){
                radiosearchdownloadresult.Add(s.FullName);
            }
        }
        RadioDisplaySearchedDownload();
    }
    
    private IEnumerator RadioPlayDownload(){
        if(RadioSource.clip.name != RadioDownloadFiles[radiodownloadnumber].FullName){
            var RadioDownloadRequest = StartCoroutine(LoadRadioDownloadFile(AbsoluteRadioPath + RadioDownloadFiles[radiodownloadnumber].FullName));
            yield return RadioDownloadRequest;
            RadioSource.Play();
            screenplaying = true;
        }
        if(RadioSource.clip.isReadyToPlay){
            RadioSource.Play();
            screenplaying = true;
        }
    }
    
    private void RadioStopDownload(){
        RadioSource.Stop();
        screenplaying = false;
    }
    
    private IEnumerator RadioPlayStream(){
        if(RadioSource.clip.name != radionewfileartistlist[radiostreamnumber] + " - " + radionewfilenamelist[radiostreamnumber] + ".mp3"){
            var RadioStreamRequest = StartCoroutine(RadioLoadStreamFile(radiostreamnumber));
            yield return RadioStreamRequest;
        }
        if(RadioSource.clip.isReadyToPlay){
            RadioSource.Play();
            screenplaying = true;
        }
    }
    
    private void RadioStopStream(){
        RadioSource.Stop();
        screenplaying = false;
    }
    
    private void RadioDownloadLoad(){
        // get all valid files
        var info = new DirectoryInfo(AbsoluteRadioPath);
        RadioDownloadFiles = info.GetFiles()
            .Where(f => RadioIsValidFileType(f.Name))
            .ToArray();
    }
 
    private bool RadioIsValidFileType(string fileName){
        return RadioValidExtensions.Contains(Path.GetExtension(fileName));
    }

    private IEnumerator LoadRadioDownloadFile(string path){        
        byte[] radioData = System.IO.File.ReadAllBytes(path);
        var decryptedfile = RadioDecrypt(radioData, Encoding.ASCII.GetBytes(radioQuasiKey), Encoding.ASCII.GetBytes(radioTokenURI));
        yield return decryptedfile;
        
        float[] f = ConvertByteToFloat(decryptedfile);
        AudioClip audioData = AudioClip.Create(Path.GetFileName(path), f.Length, 1, 44100, false, false);
        audioData.GetData(f, 0);

        WWW www = new WWW("file://" + AbsoluteRadioPath + radionewfileartistlist[radiostreamnumber] +" - " + radionewfilenamelist[radiostreamnumber] + ".mp3"); 

        AudioClip clip = www.GetAudioClip(false);
        while(!clip.isReadyToPlay)
            yield return www;
 
        RadioSource.clip = audioData;
    }

    private IEnumerator RadioLoadStreamFile(int streamnumber){
        var RadioRequest = RadioGetTokenURIRequest(streamnumber);
        yield return RadioRequest;
        WWW www = new WWW(radioTokenURI + ".mp3"); 
        AudioClip clip = www.GetAudioClip(false);
        while(!clip.isReadyToPlay)
            yield return www;
 
        clip.name = radionewfileartistlist[radiostreamnumber] +" - " + radionewfilenamelist[radiostreamnumber] + ".mp3";
        RadioSource.clip = clip;
        RadioSource.Play();
        screenplaying = true;
    }
    
    private IEnumerator RadioDisplaySearchedStream(){
        if(radiosearchstreamresult[0 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttonone.gameObject.active = true;
            radiosearchdisplaystreambuttononetext.gameObject.active = true;
            string[] split = radiosearchstreamresult[0 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[1] + "\r\n" + split[2].Replace(".mp3", "");
            radiosearchdisplaystreambuttononetext.text = tempstr;
            var RadioRequest = RadioGetTokenURIRequest(0 + (5 * radiosearchindex));
            yield return RadioRequest;
            var www = new WWW(radioURL + split[0] + ".jpg");            
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaystreambuttonone.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchstreamresult[0 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttonone.gameObject.active = false;
            radiosearchdisplaystreambuttononetext.gameObject.active = false;
            radiosearchdisplaystreambuttonone.GetComponent<Image>().sprite = null;
            radiosearchdisplaystreambuttononetext.text = ""; 
        }
        if(radiosearchstreamresult[1 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttontwo.gameObject.active = true;
            radiosearchdisplaystreambuttontwotext.gameObject.active = true;
            string[] split = radiosearchstreamresult[1 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[1] + "\r\n" + split[2].Replace(".mp3", "");
            radiosearchdisplaystreambuttontwotext.text = tempstr;
            var RadioRequest = RadioGetTokenURIRequest(1 + (5 * radiosearchindex));
            yield return RadioRequest;
            var www = new WWW(radioURL + split[0] + ".jpg");            
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaystreambuttontwo.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchstreamresult[1 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttontwo.gameObject.active = false;
            radiosearchdisplaystreambuttontwotext.gameObject.active = false;
            radiosearchdisplaystreambuttontwotext.GetComponent<Image>().sprite = null;
            radiosearchdisplaystreambuttontwotext.text = ""; 
        }
        if(radiosearchstreamresult[2 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttonthree.gameObject.active = true;
            radiosearchdisplaystreambuttonthreetext.gameObject.active = true;
            string[] split = radiosearchstreamresult[2 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[1] + "\r\n" + split[2].Replace(".mp3", "");
            radiosearchdisplaystreambuttonthreetext.text = tempstr;
            var RadioRequest = RadioGetTokenURIRequest(2 + (5 * radiosearchindex));
            yield return RadioRequest;
            var www = new WWW(radioURL + split[0] + ".jpg");            
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaystreambuttonthree.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchstreamresult[2 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttonthree.gameObject.active = false;
            radiosearchdisplaystreambuttonthreetext.gameObject.active = false;
            radiosearchdisplaystreambuttonthree.GetComponent<Image>().sprite = null;
            radiosearchdisplaystreambuttonthreetext.text = ""; 
        }
        if(radiosearchstreamresult[3 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttonfour.gameObject.active = true;
            radiosearchdisplaystreambuttonfourtext.gameObject.active = true;
            string[] split = radiosearchstreamresult[3 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[1] + "\r\n" + split[2].Replace(".mp3", "");
            radiosearchdisplaystreambuttonfourtext.text = tempstr;
            var RadioRequest = RadioGetTokenURIRequest(3 + (5 * radiosearchindex));
            yield return RadioRequest;
            var www = new WWW(radioURL + split[0] + ".jpg");            
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaystreambuttonfour.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchstreamresult[3 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttonfour.gameObject.active = false;
            radiosearchdisplaystreambuttonfourtext.gameObject.active = false;
            radiosearchdisplaystreambuttonfour.GetComponent<Image>().sprite = null;
            radiosearchdisplaystreambuttonfourtext.text = ""; 
        }
        if(radiosearchstreamresult[4 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttonfive.gameObject.active = true;
            radiosearchdisplaystreambuttonfivetext.gameObject.active = true;
            string[] split = radiosearchstreamresult[4 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[1] + "\r\n" + split[2].Replace(".mp3", "");
            radiosearchdisplaystreambuttonfivetext.text = tempstr;
            var RadioRequest = RadioGetTokenURIRequest(4 + (5 * radiosearchindex));
            yield return RadioRequest;
            var www = new WWW(radioURL + split[0] + ".jpg");            
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaystreambuttonfive.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchstreamresult[4 + (5 * radiosearchindex)] != null){
            radiosearchdisplaystreambuttonfive.gameObject.active = true;
            radiosearchdisplaystreambuttonfivetext.gameObject.active = true;
            radiosearchdisplaystreambuttonfive.GetComponent<Image>().sprite = null;
            radiosearchdisplaystreambuttonfivetext.text = "";
        }
    }
    
    private IEnumerator RadioDisplaySearchedDownload(){
        if(radiosearchdownloadresult[0 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttonone.gameObject.active = true;
            radiosearchdisplaydownloadbuttononetext.gameObject.active = true;
            string[] split = radiosearchdownloadresult[0 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[0] + "\r\n" + split[1].Replace(".mp3", "");
            radiosearchdisplaydownloadbuttononetext.text = tempstr;
            WWW www = new WWW("file://" + AbsoluteRadioPath + radiosearchdownloadresult[0 + (5 * radiosearchindex)] + ".jpg"); 
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaydownloadbuttonone.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchdownloadresult[0 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttonone.gameObject.active = false;
            radiosearchdisplaydownloadbuttononetext.gameObject.active = false;
            radiosearchdisplaydownloadbuttonone.GetComponent<Image>().sprite = null;
            radiosearchdisplaydownloadbuttononetext.text = "";            
        }
        if(radiosearchdownloadresult[1 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttontwo.gameObject.active = true;
            radiosearchdisplaydownloadbuttontwotext.gameObject.active = true;
            string[] split = radiosearchdownloadresult[1 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[0] + "\r\n" + split[1].Replace(".mp3", "");
            radiosearchdisplaydownloadbuttontwotext.text = tempstr;
            WWW www = new WWW("file://" + AbsoluteRadioPath + radiosearchdownloadresult[1 + (5 * radiosearchindex)] + ".jpg"); 
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaydownloadbuttontwo.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchdownloadresult[1 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttontwo.gameObject.active = false;
            radiosearchdisplaydownloadbuttontwotext.gameObject.active = false;
            radiosearchdisplaydownloadbuttontwo.GetComponent<Image>().sprite = null;
            radiosearchdisplaydownloadbuttontwotext.text = ""; 
        }
        if(radiosearchdownloadresult[2 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttonthree.gameObject.active = true;
            radiosearchdisplaydownloadbuttonthreetext.gameObject.active = true;
            string[] split = radiosearchdownloadresult[2 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[0] + "\r\n" + split[1].Replace(".mp3", "");
            radiosearchdisplaydownloadbuttonthreetext.text = tempstr;
            WWW www = new WWW("file://" + AbsoluteRadioPath + radiosearchdownloadresult[2 + (5 * radiosearchindex)] + ".jpg"); 
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaydownloadbuttonthree.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchdownloadresult[2 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttonthree.gameObject.active = false;
            radiosearchdisplaydownloadbuttonthreetext.gameObject.active = false;
            radiosearchdisplaydownloadbuttonthree.GetComponent<Image>().sprite = null;
            radiosearchdisplaydownloadbuttonthreetext.text = ""; 
        }
        if(radiosearchdownloadresult[3 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttonfour.gameObject.active = true;
            radiosearchdisplaydownloadbuttonfourtext.gameObject.active = true;
            string[] split = radiosearchdownloadresult[3 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[0] + "\r\n" + split[1].Replace(".mp3", "");
            radiosearchdisplaydownloadbuttonfourtext.text = tempstr;
            WWW www = new WWW("file://" + AbsoluteRadioPath + radiosearchdownloadresult[3 + (5 * radiosearchindex)] + ".jpg"); 
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaydownloadbuttonfour.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchdownloadresult[3 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttonfour.gameObject.active = false;
            radiosearchdisplaydownloadbuttonfourtext.gameObject.active = false;
            radiosearchdisplaydownloadbuttonfour.GetComponent<Image>().sprite = null;
            radiosearchdisplaydownloadbuttonfourtext.text = ""; 
        }
        if(radiosearchdownloadresult[4 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttonfive.gameObject.active = true;
            radiosearchdisplaydownloadbuttonfivetext.gameObject.active = true;
            string[] split = radiosearchdownloadresult[4 + (5 * radiosearchindex)].Split(new string[] { " - " }, StringSplitOptions.None); 
            string tempstr = split[0] + "\r\n" + split[1].Replace(".mp3", "");
            radiosearchdisplaydownloadbuttonfivetext.text = tempstr;
            WWW www = new WWW("file://" + AbsoluteRadioPath + radiosearchdownloadresult[4 + (5 * radiosearchindex)] + ".jpg"); 
            yield return www;
            Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture,rec,new Vector2(0.5f,0.5f),100);
            radiosearchdisplaydownloadbuttonfive.GetComponent<Image>().sprite = spriteToUse;
            www.Dispose();
            www = null;
        }
        if(radiosearchdownloadresult[4 + (5 * radiosearchindex)] != null){            
            radiosearchdisplaydownloadbuttonfive.gameObject.active = false;
            radiosearchdisplaydownloadbuttonfivetext.gameObject.active = false;
            radiosearchdisplaydownloadbuttonfive.GetComponent<Image>().sprite = null;
            radiosearchdisplaydownloadbuttonfivetext.text = ""; 
        }
    }
    
    //Functions
    
    private void RadioGetBalanceOfQuasi (string addressFrom){
        StartCoroutine(RadioGetBalanceOfQuasiRequest(addressFrom));
    }

    private IEnumerator RadioGetBalanceOfQuasiRequest (string addressFrom) {
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		var getBalanceOfQuasiRequest = new EthCallUnityRequest(_url);
		var getBalanceOfQuasiInput = quasiService.RadioCreateBalanceOfQuasiInput(addressFrom);
		Debug.Log("Getting balance of: " + addressFrom);
		yield return getBalanceOfQuasiRequest.SendRequest(getBalanceOfQuasiInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		if (getBalanceOfQuasiRequest.Exception == null) {
            var hexstring = getBalanceOfQuasiRequest.Result;
            if(hexstring != "0x0000000000000000000000000000000000000000000000000000000000000000"){
                var balancehbi = new HexBigInteger(hexstring);
                RadioAccountQuasiBalance = Nethereum.Util.UnitConversion.Convert.FromWei(balancehbi, 18);
                RadioBalanceQuasi = (float)RadioAccountQuasiBalance;
                Debug.Log(RadioBalanceQuasi);
            }
            if(hexstring == "0x0000000000000000000000000000000000000000000000000000000000000000"){
                RadioAccountQuasiBalance = 0;
                RadioBalanceQuasi = 0;
                Debug.Log(RadioBalanceQuasi);
            }
		} else {
			Debug.Log("Error getting balance of: " + getBalanceOfQuasiRequest.Exception.Message);
		}
	}
    
    private void RadioDepositQuasi(float transferAmount, string password) {
        Debug.Log("Trying to deposit Quasi: " + Nethereum.Util.UnitConversion.Convert.ToWei(System.Convert.ToDecimal(transferAmount), 18));
		StartCoroutine(RadioApproveQuasiRequest(Nethereum.Util.UnitConversion.Convert.ToWei(System.Convert.ToDecimal(transferAmount), 18), password));
	}
    
    private IEnumerator RadioApproveQuasiRequest (BigInteger transferAmount, string password) {
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		var transactionInput = quasiService.CreateApproveQuasiInput(
			accountAddress,
			radioAddress,
			transferAmount,
			new HexBigInteger(BigInteger.Parse(gasallowed)),
			new HexBigInteger(BigInteger.Parse(gasprice)),
			new HexBigInteger(0)
		);
		Debug.Log("Approving Quasi to: " + radioAddress);
        var aupkstr = "";
        try{
            var service = new KeyStoreService();
            var aupk = service.DecryptKeyStoreFromJson(password, accountEncryptedJson);
            aupkstr = aupk.ToHex();
        } catch (Exception e) {
            Debug.Log("Error showing PrivateKey and/or Address: " + e);
            if(e.Message.Contains("Cannot derive the same mac as the one provided from the cipher and derived key")){
                radiomessage = "Error Invalid Password or Encrypted Private Key";
            }
        }
		var transactionSignedRequest = new TransactionSignedUnityRequest(_url, aupkstr, accountAddress);
		yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
		if (transactionSignedRequest.Exception == null) {
			Debug.Log("Transfered tx created: " + transactionSignedRequest.Result);
			checkTx(transactionSignedRequest.Result, (cb) => {
                if(cb){
                    radiomessage = "Depositing Tokens";
                    StartCoroutine(RadioDepositQuasiRequest(transferAmount, password));
                }
                if(!cb){
                    radiomessage = "Transaction Failed";
                }
			});
		} else {
            if(transactionSignedRequest.Exception.Message.Contains("replacement transaction underpriced")){
                radiomessage = "Unprocessed transaction on account, increase gas limit and send again";
            }
			Debug.Log("Error approving tokens: " + transactionSignedRequest.Exception.Message);
		}
	}
    
    private IEnumerator RadioDepositQuasiRequest (BigInteger transferAmount, string password) {
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		var transactionInput = quasiService.RadioCreateDepositQuasiInput(
			accountAddress,
			new HexBigInteger(BigInteger.Parse(gasallowed)),
			new HexBigInteger(BigInteger.Parse(gasprice)),
			new HexBigInteger(0)
		);
		Debug.Log("Transfering Quasi to: " + radioAddress);
        var aupkstr = "";
        try{
            var service = new KeyStoreService();
            var aupk = service.DecryptKeyStoreFromJson(password, accountEncryptedJson);
            aupkstr = aupk.ToHex();
        } catch (Exception e) {
            Debug.Log("Error showing PrivateKey and/or Address: " + e);
            if(e.Message.Contains("Cannot derive the same mac as the one provided from the cipher and derived key")){
                radiomessage = "Error Invalid Password or Encrypted Private Key";
            }
        }
		var transactionSignedRequest = new TransactionSignedUnityRequest(_url, aupkstr, accountAddress);
		yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
		if (transactionSignedRequest.Exception == null) {
			Debug.Log("Transfered tx created: " + transactionSignedRequest.Result);
			checkTx(transactionSignedRequest.Result, (cb) => {
                if(cb){
                    radiomessage = "Tokens Delivered";
                }
                if(!cb){
                    radiomessage = "Transaction Failed";
                }
			});
		} else {
            if(transactionSignedRequest.Exception.Message.Contains("replacement transaction underpriced")){
                radiomessage = "Unprocessed transaction on account, increase gas limit and send again";
            }
			Debug.Log("Error transfering Quasi: " + transactionSignedRequest.Exception.Message);
		}
	}
    
    private void RadioWithdrawQuasi(float withdrawAmount, string password) {
        Debug.Log("Trying to withdraw Quasi");
		StartCoroutine(RadioWithdrawQuasiRequest(Nethereum.Util.UnitConversion.Convert.ToWei(System.Convert.ToDecimal(withdrawAmount), 18), password));
	}
    
    private IEnumerator RadioWithdrawQuasiRequest (BigInteger withdrawAmount, string password) {
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		var transactionInput = quasiService.RadioCreateWithdrawQuasiInput(
			accountAddress,
            withdrawAmount,
			new HexBigInteger(BigInteger.Parse(gasallowed)),
			new HexBigInteger(BigInteger.Parse(gasprice)),
			new HexBigInteger(0)
		);
        Debug.Log("Withdrawing Quasi tokens from: " + radioAddress);
        var aupkstr = "";
        try{
            var service = new KeyStoreService();
            var aupk = service.DecryptKeyStoreFromJson(password, accountEncryptedJson);
            aupkstr = aupk.ToHex();
        } catch (Exception e) {
            Debug.Log("Error showing PrivateKey and/or Address: " + e);
            if(e.Message.Contains("Cannot derive the same mac as the one provided from the cipher and derived key")){
                radiomessage = "Error Invalid Password or Encrypted Private Key";
            }
        }
		var transactionSignedRequest = new TransactionSignedUnityRequest(_url, aupkstr, accountAddress);
		yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
		if (transactionSignedRequest.Exception == null) {
			Debug.Log("Withdraw Quasi tx created: " + transactionSignedRequest.Result);
			checkTx(transactionSignedRequest.Result, (cb) => {
                if(cb){
                    radiomessage = "Tokens withrawled";
                }
                if(!cb){
                    radiomessage = "Transaction Failed";
                }
			});
		} else {
            if(transactionSignedRequest.Exception.Message.Contains("replacement transaction underpriced")){
                radiomessage = "Unprocessed transaction on account, increase gas limit and send again";
            }
			Debug.Log("Error withdraw Quasi order: " + transactionSignedRequest.Exception.Message);
		}
	}
    
    private void RadioCreateCopy(string fileId, string password) {
        Debug.Log("Trying to create copy");
		StartCoroutine(RadioCreateCopyRequest(Nethereum.Util.UnitConversion.Convert.ToWei(System.Convert.ToDecimal(fileId), 18), password));
	}
    
    private IEnumerator RadioCreateCopyRequest (BigInteger fileId, string password) {
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		var transactionInput = quasiService.RadioCreateCopyInput(
			accountAddress,
            fileId,
			new HexBigInteger(BigInteger.Parse(gasallowed)),
			new HexBigInteger(BigInteger.Parse(gasprice)),
			new HexBigInteger(0)
		);
        Debug.Log("Creating copy of: " + fileId);
        var aupkstr = "";
        try{
            var service = new KeyStoreService();
            var aupk = service.DecryptKeyStoreFromJson(password, accountEncryptedJson);
            aupkstr = aupk.ToHex();
        } catch (Exception e) {
            Debug.Log("Error showing PrivateKey and/or Address: " + e);
            if(e.Message.Contains("Cannot derive the same mac as the one provided from the cipher and derived key")){
                radiomessage = "Error Invalid Password or Encrypted Private Key";
            }
        }
		var transactionSignedRequest = new TransactionSignedUnityRequest(_url, aupkstr, accountAddress);
		yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
		if (transactionSignedRequest.Exception == null) {
			Debug.Log("Create Copy tx created: " + transactionSignedRequest.Result);
			checkTx(transactionSignedRequest.Result, (cb) => {
                if(cb){
                    radiomessage = "Token Copy Created";
                    StartCoroutine(RadioGetQuasiKeyRequest());
                    StartCoroutine(RadioGetTokenURIRequest((int)fileId));
                    StartCoroutine(RadioInitiateDownload(fileId));
                    
                }
                if(!cb){
                    radiomessage = "Transaction Failed";
                }
			});
		} else {
            if(transactionSignedRequest.Exception.Message.Contains("replacement transaction underpriced")){
                radiomessage = "Unprocessed transaction on account, increase gas limit and send again";
            }
			Debug.Log("Error creating copy: " + transactionSignedRequest.Exception.Message);
		}
	}
    
    private IEnumerator RadioInitiateDownload(BigInteger fileId){
        while(radioQuasiKey == ""){
        }
        while(radioTokenURI == ""){
        }
        if (!System.IO.Directory.Exists(AbsoluteRadioPath)){
            System.IO.Directory.CreateDirectory(AbsoluteRadioPath);
        }
        var RadioRequest = RadioGetTokenURIRequest((int)fileId);
        yield return RadioRequest;
        WWW www1 = new WWW(radioURL + "/" + fileId.ToString() + ".mp3");
        yield return www1;
        var encryptedfile = RadioEncrypt(www1.bytes, Encoding.ASCII.GetBytes(radioQuasiKey), Encoding.ASCII.GetBytes(radioTokenURI));
        yield return encryptedfile;
        
        System.IO.File.WriteAllBytes(AbsoluteRadioPath + radionewfileartistlist[(int)fileId] + " - " + radionewfilenamelist[(int)fileId] + ".mp3", encryptedfile);
        
        WWW www2 = new WWW(radioURL + "/" + fileId.ToString() + ".jpg");
        yield return www2;
        
        System.IO.File.WriteAllBytes(AbsoluteRadioPath + radionewfileartistlist[(int)fileId] + " - " + radionewfilenamelist[(int)fileId] + ".jpg", www2.bytes);
    }
    
    public byte[] RadioEncrypt(byte[] UnencryptedData, byte[] Key, byte[] IV)
    {
        RijndaelManaged rm = new RijndaelManaged();
        ICryptoTransform EncryptorTransform = rm.CreateEncryptor(Key, IV);

        byte[] bytes = UnencryptedData;
        MemoryStream memoryStream = new MemoryStream();
        
        #region Write the decrypted value to the encryption stream
        CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
        cs.Write(bytes, 0, bytes.Length);
        cs.FlushFinalBlock();
        #endregion

        #region Read encrypted value back out of the stream
        memoryStream.Position = 0;
        byte[] encrypted = new byte[memoryStream.Length];
        memoryStream.Read(encrypted, 0, encrypted.Length);
        #endregion

        cs.Close();
        memoryStream.Close();
        EncryptorTransform = null;
        return encrypted;
    }


    public byte[] RadioDecrypt(byte[] EncryptedData, byte[] Key, byte[] IV)
    {
        RijndaelManaged rm = new RijndaelManaged();
        ICryptoTransform DecryptorTransform = rm.CreateDecryptor(Key, IV);
        
        #region Write the encrypted value to the decryption stream
        MemoryStream encryptedStream = new MemoryStream();
        CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
        decryptStream.Write(EncryptedData, 0, EncryptedData.Length);
        decryptStream.FlushFinalBlock();
        #endregion

        #region Read the decrypted value from the stream.
        encryptedStream.Position = 0;
        Byte[] decrypted = new Byte[encryptedStream.Length];
        encryptedStream.Read(decrypted, 0, decrypted.Length);
        encryptedStream.Close();
        DecryptorTransform = null;
        #endregion
        return decrypted;
    }
    
    private void RadioCreateStreamRenewal(string password) {
        Debug.Log("Trying to create stream renewal");
		StartCoroutine(RadioCreateStreamRenewalRequest(password));
	}
    
    private IEnumerator RadioCreateStreamRenewalRequest (string password) {
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		var transactionInput = quasiService.RadioCreateStreamRenewalInput(
			accountAddress,
			new HexBigInteger(BigInteger.Parse(gasallowed)),
			new HexBigInteger(BigInteger.Parse(gasprice)),
			new HexBigInteger(0)
		);
        Debug.Log("Creating stream renewal of: " + accountAddress);
        var aupkstr = "";
        try{
            var service = new KeyStoreService();
            var aupk = service.DecryptKeyStoreFromJson(password, accountEncryptedJson);
            aupkstr = aupk.ToHex();
        } catch (Exception e) {
            Debug.Log("Error showing PrivateKey and/or Address: " + e);
            if(e.Message.Contains("Cannot derive the same mac as the one provided from the cipher and derived key")){
                radiomessage = "Error Invalid Password or Encrypted Private Key";
            }
        }
		var transactionSignedRequest = new TransactionSignedUnityRequest(_url, aupkstr, accountAddress);
		yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
		if (transactionSignedRequest.Exception == null) {
			Debug.Log("Create Copy tx created: " + transactionSignedRequest.Result);
			checkTx(transactionSignedRequest.Result, (cb) => {
                if(cb){
                    radiomessage = "Stream Renewed";
                }
                if(!cb){
                    radiomessage = "Transaction Failed";
                }
			});
		} else {
            if(transactionSignedRequest.Exception.Message.Contains("replacement transaction underpriced")){
                radiomessage = "Unprocessed transaction on account, increase gas limit and send again";
            }
			Debug.Log("Error renewing stream: " + transactionSignedRequest.Exception.Message);
		}
	}

    private IEnumerator RadioGetQuasiKeyRequest () {
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		var getBalanceOfQuasiRequest = new EthCallUnityRequest(_url);
		var getBalanceOfQuasiInput = quasiService.RadioCreateQuasiKeyInput();
		Debug.Log("Getting Quasi Key");
		yield return getBalanceOfQuasiRequest.SendRequest(getBalanceOfQuasiInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		if (getBalanceOfQuasiRequest.Exception == null) {
            var hexstring = getBalanceOfQuasiRequest.Result;
            if(hexstring != "0x0000000000000000000000000000000000000000000000000000000000000000"){
                var stringqzi = ConvertHexToString(hexstring);
                yield return stringqzi;
                radioQuasiKey = stringqzi;
                //Debug.Log(radioQuasiKey);
            }
            if(hexstring == "0x0000000000000000000000000000000000000000000000000000000000000000"){
                radioQuasiKey = "";
                //Debug.Log(radioQuasiKey);
            }
		} else {
			Debug.Log("Error getting balance of: " + getBalanceOfQuasiRequest.Exception.Message);
		}
	}
    
    public IEnumerator RadioGetTokenURIRequest(int streamnumber){
        BigInteger streaminteger = Nethereum.Util.UnitConversion.Convert.ToWei(System.Convert.ToDecimal(streamnumber), 18);
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		var getTokenURIRequest = new EthCallUnityRequest(_url);
		var getTokenURIInput = quasiService.RadioCreateTokenURIInput(streaminteger);
		Debug.Log("Getting total supply");
		yield return getTokenURIRequest.SendRequest(getTokenURIInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		if (getTokenURIRequest.Exception == null) {
            var hexstring = getTokenURIRequest.Result;
            var stringqzi = ConvertHexToString(hexstring);
            string[] partstring = stringqzi.Split("/".ToCharArray());
            var tempstring = "";
            for(int x = 0; x < partstring.Length - 1; x++){
                tempstring = tempstring + partstring[x];
            }
            radioURL = tempstring;
            radioTokenURI = stringqzi;
            Debug.Log(radioTokenURI);
		} else {
			Debug.Log("Error getting token URI: " + getTokenURIRequest.Exception.Message);
		}
	}
    
    public IEnumerator RadioGetNewFileEventsRequest(){
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
        var getNewFileEventsRequest = new EthGetLogsUnityRequest(_url);
		var getNewFileEventsInput = quasiService.RadioCreateNewFileEventsInput();
		Debug.Log("Checking Synthetic events from " + erc721TokenAddress);
		yield return getNewFileEventsRequest.SendRequest(getNewFileEventsInput);
		if (getNewFileEventsRequest.Exception == null) {
            var result = getNewFileEventsRequest.Result;
            Debug.Log(result + " is result");
            var newFileEventFunction = quasiService.RadioNewFileEvents();
            var newfileeventlistobj = newFileEventFunction.DecodeAllEventsForEvent<RadioNewFileEventsDTO>(result);
            radionewfilelist = new List<float>();
            radionewfilenamelist = new List<string>();
            radionewfileartistlist = new List<string>();
            radionewfiledict = new Dictionary<BigInteger, RadioNewFileClass>();
            for(var x = 0; x < result.Length; x++){
                Debug.Log(x + " " + newfileeventlistobj[x].Event.FileId);
                radionewfiledict[newfileeventlistobj[x].Event.FileId] = new RadioNewFileClass(newfileeventlistobj[x].Event.FileId, newfileeventlistobj[x].Event.Name, newfileeventlistobj[x].Event.Artist);
            }
            var myList = radionewfiledict.ToList();
            for(var y = 0; y < myList.Count; y++){
                radionewfilelist.Add(float.Parse(myList[y].Value.fileId.ToString()));
                radionewfilenamelist.Add(myList[y].Value.name);
                radionewfileartistlist.Add(myList[y].Value.artist);
            }
		} else {
			Debug.Log("Error getting Synthetic events: " + getNewFileEventsRequest.Exception.Message);
		}
        return true;
    }
    
    [FunctionOutput]
    public class RadioNewFileEventsDTO
    {
        [Parameter("uint256", 1)]
        public BigInteger FileId { get; set; }
        
        [Parameter("string", 2)]
        public string Name { get; set; }
        
        [Parameter("string", 3)]
        public string Artist { get; set; }
        
    }
    
     public IEnumerator RadioGetNewTokenEventsRequest(){
        yield return new WaitForSeconds(0.5f);
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
        var getNewTokenEventsRequest = new EthGetLogsUnityRequest(_url);
		var getNewTokenEventsInput = quasiService.RadioCreateNewTokenEventsInput();
		Debug.Log("Checking Synthetic events from " + erc721TokenAddress);
		yield return getNewTokenEventsRequest.SendRequest(getNewTokenEventsInput);
		if (getNewTokenEventsRequest.Exception == null) {
            var result = getNewTokenEventsRequest.Result;
            Debug.Log(result + " is result");
            var newTokenEventFunction = quasiService.RadioNewTokenEvents();
            var newtokeneventlistobj = newTokenEventFunction.DecodeAllEventsForEvent<RadioNewTokenEventsDTO>(result);
            radionewtokenlist = new List<float>();
            radionewtokennamelist = new List<string>();
            radionewtokenartistlist = new List<string>();
            radionewtokendict = new Dictionary<BigInteger, RadioNewTokenClass>();
            for(var x = 0; x < result.Length; x++){
                Debug.Log(x + " " + newtokeneventlistobj[x].Event.TokenId);
                radionewtokendict[newtokeneventlistobj[x].Event.TokenId] = new RadioNewTokenClass(newtokeneventlistobj[x].Event.TokenId, newtokeneventlistobj[x].Event.Name, newtokeneventlistobj[x].Event.Artist);
            }
            var myList = radionewtokendict.ToList();
            for(var y = 0; y < myList.Count; y++){
                radionewtokenlist.Add(float.Parse(myList[y].Value.tokenId.ToString()));
                radionewtokennamelist.Add(myList[y].Value.name);
                radionewtokenartistlist.Add(myList[y].Value.artist);
            }
		} else {
			Debug.Log("Error getting Synthetic events: " + getNewTokenEventsRequest.Exception.Message);
		}
        return true;
    }
    
    [FunctionOutput]
    public class RadioNewTokenEventsDTO
    {
        [Parameter("uint256", 1)]
        public BigInteger TokenId { get; set; }
        
        [Parameter("string", 2)]
        public string Name { get; set; }
        
        [Parameter("string", 3)]
        public string Artist { get; set; }
        
    }
    
    private static string ConvertHexToString(string HexValue){
        var StrValue = "";
        while(HexValue.Length > 0){
            StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
            HexValue = HexValue.Substring(2, HexValue.Length - 2);
        }
        return StrValue;
    }
    
    private static float[] ConvertByteToFloat(byte[] array) {
        float[] floatArr = new float[array.Length / 4];

        for (int i = 0; i < floatArr.Length; i++) {
            if (BitConverter.IsLittleEndian){
                Array.Reverse(array, i * 4, 4);
            }
            floatArr[i] = BitConverter.ToSingle(array, i * 4);
        }

        return floatArr;
    }
    
    public void checkTx(string txHash, Action<bool> callback) {
        QuasiService quasiService = new QuasiService(ERC721ExchangeAddress, erc721TokenAddress, erc20TokenAddress, radioAddress, televisionAddress, workAddress, buyItemAddress, advertisementAddress, multiexchange, false);
		StartCoroutine(quasiService.CheckTransactionReceiptIsMined(
			_url,
			txHash,
			(cb) => {
				Debug.Log("The transaction has been mined succesfully");
				callback(true);
			}
		));
        quasiService = null;
	}
}
