using MyLittleOcean.Models.Follow;

public class FollowService : IFollowService {
    private readonly IFollowRepository _followRepository;
    public FollowService(IFollowRepository followRepository) {
        this._followRepository = followRepository;
    }

    /// <summary>
    ///     팔로우 하기
    /// </summary>
    /// <param name="param"> targetUserNo : 대상 유저 No, followUserNo : 팔로우 유저 No</param>
    public int CreateFollow(FollowCreateRequestDto param) {
        return _followRepository.CreateFollow(param);
    }

    /// <summary>
    ///     팔로우 취소
    /// </summary>
    /// <param name="param">targetUserNo : 대상 유저 No, followUserNo : 팔로우 유저 No</param>
    public int DeleteFollow(FollowCreateRequestDto param) {
        return _followRepository.DeleteFollow(param);
    }

    /// <summary>
    ///     특정 유저가 팔로우 한 목록
    /// </summary>
    /// <param name="userNo">유저 식별 번호</param>
    /// <returns>List<{UserNo, UserName}></returns>
    public List<FollowDetail> GetFollowingList(int userNo) {
        return _followRepository.GetFollowingList(userNo);
    }

    /// <summary>
    /// 특정 유저를 팔로우한 목록
    /// </summary>
    /// <param name="userNo">유저 식별 번호</param>
    /// <returns>List<{UserNo, UserName}></returns>
    public List<FollowDetail> GetFollowersList(int userNo) {
        return _followRepository.GetFollowersList(userNo);
    }
}
