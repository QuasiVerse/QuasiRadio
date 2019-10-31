pragma solidity ^0.4.18;

library strings {
    
    function appendUintToString(string inStr, uint v) internal pure returns (string str) {
        uint maxlength = 100;
        bytes memory reversed = new bytes(maxlength);
        uint i = 0;
        while (v != 0) {
            uint remainder = v % 10;
            v = v / 10;
            reversed[i++] = byte(48 + remainder);
        }
        bytes memory inStrb = bytes(inStr);
        bytes memory s = new bytes(inStrb.length + i + 1);
        uint j;
        for (j = 0; j < inStrb.length; j++) {
            s[j] = inStrb[j];
        }
        for (j = 0; j <= i; j++) {
            s[j + inStrb.length] = reversed[i - j];
        }
        str = string(s);
    }
}

library SafeMath {
    
    function safeMul(uint256 a, uint256 b) internal pure returns (uint256) {
        if (a == 0) {
            return 0;
        }
        uint256 c = a * b;
        assert(c / a == b);
        return c;
    }

    function safeDiv(uint256 a, uint256 b) internal pure returns (uint256) {
        uint256 c = a / b;
        return c;
    }

    function safeSub(uint256 a, uint256 b) internal pure returns (uint256) {
        assert(b <= a);
        return a - b;
    }

    function safeAdd(uint256 a, uint256 b) internal pure returns (uint256) {
        uint256 c = a + b;
        assert(c >= a);
        return c;
    }
}


interface ERC165 {

    function supportsInterface(bytes4 interfaceID) external view returns (bool);
    
}

interface ERC20 {
    function transfer(address _to, uint _value) returns (bool);
    function transferFrom(address _from, address _to, uint _value) returns (bool);
    function balanceOf(address _owner) constant returns (uint balance);
    function allowance(address _owner, address _spender) constant returns (uint remaining);
}

interface ERC721TokenReceiver {

	function onERC721Received(address _from, uint256 _tokenId, bytes data) external returns(bytes4);
    
}

interface  ERC721 {

    event Transfer(address indexed _from, address indexed _to, uint256 _tokenId);
    event Approval(address indexed _owner, address indexed _approved, uint256 _tokenId);
    event ApprovalForAll(address indexed _owner, address indexed _operator, bool _approved);

    function balanceOf(address _owner) external view returns (uint256);
    function ownerOf(uint256 _tokenId) external view returns (address);
    function safeTransferFrom(address _from, address _to, uint256 _tokenId, bytes data) external payable;
    function safeTransferFrom(address _from, address _to, uint256 _tokenId) external payable;
    function transferFrom(address _from, address _to, uint256 _tokenId) external payable;
    function approve(address _approved, uint256 _tokenId) external payable;
    function setApprovalForAll(address _operator, bool _approved) external;
    function getApproved(uint256 _tokenId) external view returns (address);
    function isApprovedForAll(address _owner, address _operator) external view returns (bool);
    
}

interface ERC721Metadata {

    function name() external pure returns (string _name);
    function symbol() external pure returns (string _symbol);
    function tokenURI(uint256 _tokenId) external view returns (string);
    
}

interface ERC721Enumerable {

    function totalSupply() external view returns (uint256);
    function tokenByIndex(uint256 _index) external view returns (uint256);
    function tokenOfOwnerByIndex(address _owner, uint256 _index) external view returns (uint256);
}

contract Ownable {

    address public owner;

    function Ownable() {
        owner = msg.sender;
    }

    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }

    function transferOwnership(address newOwner) onlyOwner {
        require(newOwner != address(0));
        owner = newOwner;
    }
    
}

contract QuasiRadioFactory is Ownable {
    
    using SafeMath for uint256;

    ERC20 public _quasiCoin;
    
    uint private _quasiKey;
    
    uint public _utilityRate;
    uint public _utilityFileRate;
    
    uint public _totalSupply;
    uint public _totalFileSupply;
    
    mapping (uint => uint) file;
    mapping (uint => address) fileOwner;
    mapping (address => uint) ownerFileCount;
    
    mapping (uint => uint) token;
    mapping (uint => address) tokenOwner;
    mapping (address => uint) ownerTokenCount;
    
    mapping (address => uint32) renewalTimer;
    
    mapping (address => uint) userQuasiBalance;
    
    mapping (uint => address) tokenApprovals;
    mapping (address => address) tokenApprovalsForAll;

    
    event NewFile(uint FileId, string Name, string Artist);
    event NewToken(uint TokenId, uint FileId);
    event QuasiDeposit(address indexed FromAddress, uint DepositAmount);
    event QuasiWithdraw(address indexed FromAddress, uint WithdrawAmount);
    
    function utility(address _quasiCoinAddr) public onlyOwner {
        _quasiCoin = ERC20(_quasiCoinAddr);
    }
    
    function utilityRate(uint _rate) public onlyOwner {
        _utilityRate = _rate;
    }
    
    function utilityFileRate(uint _rate) public onlyOwner {
        _utilityFileRate = _rate;
    }
        
    function getBalanceQuasi(address userAddress) public constant returns (uint balance){
        return userQuasiBalance[userAddress];
    }
    
    function depositQuasi() public {
        uint weiAmount = _quasiCoin.allowance(msg.sender, this);
        require (weiAmount > 0);
        require (_quasiCoin.balanceOf(msg.sender) >= weiAmount);
        require (_quasiCoin.transferFrom(msg.sender, this, weiAmount));
        userQuasiBalance[msg.sender] = userQuasiBalance[msg.sender].safeAdd(weiAmount);
        emit QuasiDeposit(msg.sender, weiAmount);
    }
    
    function withdrawQuasi(uint withdrawAmount) public {
        uint weiAmount = userQuasiBalance[msg.sender];
        require (weiAmount > 0);
        require (weiAmount >= withdrawAmount);
        require (_quasiCoin.transferFrom(this, msg.sender, withdrawAmount));
        userQuasiBalance[msg.sender] = userQuasiBalance[msg.sender].safeSub(withdrawAmount);
        emit QuasiWithdraw(msg.sender, withdrawAmount);
    }
    
    function copyFile(address _owner, uint _fileId) private {
        uint _tokenId = _totalSupply.safeAdd(1);
        tokenOwner[_tokenId] = _owner;
        ownerTokenCount[_owner] = ownerTokenCount[_owner].safeAdd(1);
        token[_tokenId] = file[_fileId];
        _totalSupply = _tokenId;
        emit NewToken(_tokenId, _fileId);
    }

    function createFile(string _name, string _artist, address _owner) public onlyOwner {
        uint _fileId = _totalFileSupply.safeAdd(1);
        fileOwner[_fileId] = _owner;
        ownerFileCount[_owner] = ownerFileCount[_owner].safeAdd(1);
        file[_fileId] = _fileId;
        _totalFileSupply = _fileId;
        emit NewFile(_fileId, _name, _artist);
    }
    
    function copy(uint _fileId) external {
        require(userQuasiBalance[msg.sender] >= _utilityFileRate);
        userQuasiBalance[msg.sender] = userQuasiBalance[msg.sender].safeSub(_utilityFileRate);
        copyFile(msg.sender, _fileId);
    }
    
    function streamRenewal() public {
        require(!isRenewed(msg.sender));
        require(userQuasiBalance[msg.sender] >= _utilityRate);
        userQuasiBalance[msg.sender] = userQuasiBalance[msg.sender].safeSub(_utilityRate);
        renewalTimer[msg.sender] = uint32(now + 30 days);
    }

    function isRenewed(address _sender) public view returns (bool) {
        return (renewalTimer[_sender] <= now);
    }
    
    function getQuasiKey() public view returns (uint) {
        require(isRenewed(msg.sender));
        return _quasiKey;
    }
    
    function updateQuasiKey(uint _key) public onlyOwner {
        _quasiKey = _key;
    }
}

contract QuasiRadio is QuasiRadioFactory, ERC721, ERC721Metadata, ERC721Enumerable {

    using SafeMath for uint256;
    using strings for string;

    string public symbol = "QZI";

    string public name = "Quasi Coin";
    
    string public URI;
    
    function name() external pure returns (string _name){
        string memory pureName = "Quasi Coin";
        return pureName;
    }
    
    function symbol() external pure returns (string _symbol){
        string memory pureSymbol = "QZI";
        return pureSymbol;
    }
    
    function updateURI(string newURI) public onlyOwner {
        URI = newURI;
    }
    
    function tokenURI(uint256 _tokenId) external view returns (string){
        string memory newstr = URI.appendUintToString(file[token[_tokenId]]);
        return newstr;
    }
    
    function totalSupply() public constant returns (uint256) {
        return _totalSupply;
    }
    
    function tokenByIndex(uint256 _index) external view returns (uint256){
        uint256 tokenCount = ownerTokenCount[msg.sender];

        if (tokenCount == 0) {
            return uint(0);
        } else {
            uint256[] memory result = new uint256[](tokenCount);
            uint256 totalTokens = totalSupply();
            uint256 resultIndex = 0;

            uint256 tokenId;

            for (tokenId = 1; tokenId <= totalTokens; tokenId++) {
                if (tokenOwner[tokenId] == msg.sender) {
                    result[resultIndex] = tokenId;
                    resultIndex++;
                }
            }

            return result[_index];
        }      
    }
    
    function tokenOfOwnerByIndex(address _owner, uint256 _index) external view returns (uint256) {
        uint256 tokenCount = ownerTokenCount[_owner];

        if (tokenCount == 0) {
            return uint(0);
        } else {
            uint256[] memory result = new uint256[](tokenCount);
            uint256 totalTokens = totalSupply();
            uint256 resultIndex = 0;

            uint256 tokenId;

            for (tokenId = 1; tokenId <= totalTokens; tokenId++) {
                if (tokenOwner[tokenId] == _owner) {
                    result[resultIndex] = tokenId;
                    resultIndex++;
                }
            }

            return result[_index];
        }   
    }
    
    function balanceOf(address _owner) external view returns (uint256 _balance) {
        return ownerTokenCount[_owner];
    }

    function ownerOf(uint256 _tokenId) external view returns (address _owner) {
        return tokenOwner[_tokenId];
    }
    
    function safeTransferFrom(address _from, address _to, uint256 _tokenId, bytes data) external payable {
        require(tokenApprovals[_tokenId] == msg.sender || tokenOwner[_tokenId] == msg.sender || tokenApprovalsForAll[tokenOwner[_tokenId]] == msg.sender);
        require(_to != 0);
        uint codeLength;
        assembly { codeLength := extcodesize(_to) }
        require(codeLength > 0);
        tokenOwner[_tokenId] = _to;
        tokenApprovals[_tokenId] = _to;
        ERC721TokenReceiver receiver = ERC721TokenReceiver(_to);
        receiver.onERC721Received(msg.sender, _tokenId, data);
        ownerTokenCount[_to] = ownerTokenCount[_to].safeAdd(1);
        ownerTokenCount[_from] = ownerTokenCount[_from].safeSub(1);
        emit Transfer(_from, _to, _tokenId);
    }
    
    function safeTransferFrom(address _from, address _to, uint256 _tokenId) external payable {
        require(tokenApprovals[_tokenId] == msg.sender || tokenOwner[_tokenId] == msg.sender || tokenApprovalsForAll[tokenOwner[_tokenId]] == msg.sender);
        require(_to != 0);
        uint codeLength;
        bytes memory data;
        assembly { codeLength := extcodesize(_to) }
        require(codeLength > 0);
        tokenOwner[_tokenId] = _to;
        tokenApprovals[_tokenId] = _to;
        ERC721TokenReceiver receiver = ERC721TokenReceiver(_to);
        receiver.onERC721Received(msg.sender, _tokenId, data);
        ownerTokenCount[_to] = ownerTokenCount[_to].safeAdd(1);
        ownerTokenCount[_from] = ownerTokenCount[_from].safeSub(1);
        emit Transfer(_from, _to, _tokenId);
    }
    
    function transferFrom(address _from, address _to, uint256 _tokenId) external payable {
        require(tokenApprovals[_tokenId] == msg.sender || tokenOwner[_tokenId] == msg.sender || tokenApprovalsForAll[tokenOwner[_tokenId]] == msg.sender);
        require(_to != 0);
        tokenOwner[_tokenId] = _to;
        tokenApprovals[_tokenId] = _to;
        ownerTokenCount[_to] = ownerTokenCount[_to].safeAdd(1);
        ownerTokenCount[_from] = ownerTokenCount[_from].safeSub(1);
        emit Transfer(_from, _to, _tokenId);
    }

    function approve(address _approved, uint256 _tokenId) external payable {
        require(tokenApprovals[_tokenId] == msg.sender || tokenOwner[_tokenId] == msg.sender);
        tokenApprovals[_tokenId] = _approved;
        emit Approval(msg.sender, _approved, _tokenId);
    }

    function setApprovalForAll(address _operator, bool _approved) external {
        if(_approved){
            tokenApprovalsForAll[msg.sender] = _operator;
        }
        if(!_approved){
            tokenApprovalsForAll[msg.sender] = msg.sender;
        }
        emit ApprovalForAll(msg.sender, _operator, _approved);
    }
    
    function getApproved(uint256 _tokenId) external view returns (address) {
        return tokenApprovals[_tokenId];
    }
    
    function isApprovedForAll(address _owner, address _operator) external view returns (bool) {
        bool result = false;
        if(tokenApprovalsForAll[_owner] == _operator){
            result = true;
        }
        return result;
    }
}