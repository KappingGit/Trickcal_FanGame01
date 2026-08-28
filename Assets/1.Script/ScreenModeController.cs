using System.Collections;
using System.Collections.Generic; // List를 사용하기 위해 필요합니다.
using UnityEngine;
using TMPro; // 드롭다운을 사용하기 위해 필수!

public class ScreenModeController : MonoBehaviour
{
    [Header("화면 설정 UI")]
    [SerializeField] private TMP_Dropdown screenModeDropdown; // 인스펙터에서 드롭다운 연결

    private void Awake()
    {
        InitDropdownOptions(); // 드롭다운 글자 셋업

        // 게임 시작 시, 현재 화면 상태를 드롭다운에 맞춰주는 초기화 작업 (선택사항이지만 권장)
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
            screenModeDropdown.value = 0;
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            screenModeDropdown.value = 1;
        else if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
            screenModeDropdown.value = 2;

        // 드롭다운의 값이 변할 때마다 'ChangeScreenMode' 함수가 자동으로 실행되도록 연결
        screenModeDropdown.onValueChanged.AddListener(ChangeScreenMode);
    }

    // 드롭다운에서 항목을 선택하면, 그 순서(index)가 매개변수로 들고온다
    public void ChangeScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                // 0번 : 창 모드
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("창 모드로 변경되었습니다.");
                break;

            case 1:
                // 1번 : 전체 창 모드 (테두리 없음)
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Debug.Log("전체 창 모드로 변경되었습니다.");
                break;

            case 2:
                // 2번 : 전체 화면
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Debug.Log("전체 화면으로 변경되었습니다.");
                break;
        }
    }

    // 드롭다운의 항목 이름을 설정하는 함수
    private void InitDropdownOptions()
    {
        // 1. 기존에 있던 Option A, Option B 등을 전부 날려버립니다.
        screenModeDropdown.ClearOptions();

        // 2. 우리가 넣고 싶은 이름표를 List(보따리)로 만듭니다. (순서가 0, 1, 2가 됨)
        List<string> modeOptions = new List<string>()
        {
            "창 모드",
            "전체 창 모드",
            "전체 화면"
        };

        // 3. 만든 리스트를 드롭다운에 쏙 집어넣습니다.
        screenModeDropdown.AddOptions(modeOptions);
    }

}