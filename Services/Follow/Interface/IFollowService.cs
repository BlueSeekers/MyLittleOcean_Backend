using MyLittleOcean.Models.Follow;

public interface IFollowService {

    //팔로우 하기
    int CreateFollow(FollowCreateRequestDto param);

    //팔로우 끊기
    int DeleteFollow(FollowCreateRequestDto param);

    //특정 유저가 팔로우한 목록
    List<FollowDetail> GetFollowingList(int userNo);

    //특정 유저를 팔로우한 목록
    List<FollowDetail> GetFollowersList(int userNo);

}
