
using Swashbuckle.AspNetCore.Annotations;

public class UserData {

    private int coinAmount;

    private int tokenAmount;

    [SwaggerSchema("인게임 재화(coin) 개수")]
    public int CoinAmount { get => coinAmount; set => coinAmount = value; }

    [SwaggerSchema("입장 재화(token) 개수")]
    public int TokenAmount { get => tokenAmount; set => tokenAmount = value; }
}
