# BGRA32Changer
#### 이미지의 BGRA값을 조정하고, 해당 이미지를 선형으로 합치는 프로그램입니다. 
<i>for english users: <a href="/README_en.md">readme_en.md</a>
<br/>

## 프로그램 사용 가이드
<br/>

### 1. 첫 화면
    프로그램 기동 시 화면입니다. 

<img src="/Guide images/1.png"/>

    화면 상단의 하얀색 공간에는 우리가 불러온 이미지가 자리하게 됩니다.

    화면 좌하단에 Red, Green, Blue, Alpha값을 변경할 수 있는 슬라이더가 보입니다.
    슬라이더를 조작하여 이미지의 전체적인 색깔을 변경할 수 있습니다.

    화면 우하단에는
    이미지 불러오기 버튼 및 불러 온 이미지의 갯수
    배경색 확인 및 변경을 위한 상자
    배경색 Alpha(투명도) 값을 변경할 수 있는 슬라이더
    이미지 저장 버튼과 한 줄에 출력되는 이미지의 갯수를 설정할 수 있는 상자가 있습니다.
    
### 2. 이미지 로드
<img src="/Guide images/2.png"/>

    화면 우하단의 Load Images버튼을 통해 이미지를 불러와봅시다.
<img src="/Guide images/3.png"/>

    이미지가 잘 불러와졌다면 성공입니다!
    이제 이미지를 클릭해서 색을 변경해 봅시다.
    
### 3. 색깔을 바꿔보자.
    이제 이미지의 색깔을 바꿔보겠습니다.
    먼저 좌측 하단의 Red 슬라이더를 조작해 색상을 변경해봅시다.
<img src="/Guide images/4.png"/>

<img src="/Guide images/5.png"/>
    
    슬라이더를 우측으로 옮기면 이미지에 옮긴 수치만큼의 값이 추가되게 됩니다.

    특히 이미지가 흰색 배경에 검정색 이미지만 존재하게 될 경우 배경색은 흰색(#FFFFFF) 그대로인 상태에서 
    선의 색 (#000000)에만 값이 추가되기 때문에 선의 색깔만 바뀌는 것을 볼 수 있습니다.

    위와 같은 상황에서 슬라이더를 좌측으로 움직이면 선의 색(#000000)에서는 더 이상 뺄 수 있는 값이 없으므로 
    배경 색(#FFFFFF)에서만 값을 감소시키기 때문에 배경색이 변경되는 효과를 볼 수 있습니다.

    R, G, B 값을 동일한 값만큼 우측으로 조작하면 이미지의 전체적인 밝기가 상승하게 됩니다.
    (반대로 동일한 값만큼 좌측으로 조작하면 밝기가 감소합니다.)
    
### 4. 투명도를 변경!
    이미지의 투명도 역시 변경할 수 있습니다.
    좌측 하단의 Alpha 슬라이더를 조작하면 투명도를 변경할 수 있습니다.
<img src="/Guide images/6.png"/>
    
    만약 투명한 이미지를 불러 올 경우 슬라이더를 우측으로 조작하면 불투명한 이미지로 만들 수 있습니다.
### 5. 배경색을 바꿔보자!
    현재 수정중인 이미지가 투명할 경우 뒷 배경색이 중요한 경우가 있습니다.
    
    이 배경색 변경 기능을 사용하면 손쉽게 특정 색상이 추가된 이미지를 만들 수 있습니다.

    우하단에 있는 Background Colour 상자를 클릭해봅시다. (기본색상은 흰색입니다.)
<img src="/Guide images/7.png"/>

    원하는 색상을 설정하고 확인 버튼을 누르면 이미지 뒤의 배경색이 변경됩니다.
<img src="/Guide images/8.png"/>

    해당 색상은 이미지를 저장할 때 이미지에 같이 적용됩니다.
### 6. 배경색의 투명도를 변경해 투명한 이미지를 만들어보자.
    배경색을 지정한 상태에서 투명한 이미지를 만들고 싶다면 우하단의 Alpha 슬라이더를 조작해봅시다.
<img src="/Guide images/9.png"/>

    슬라이더를 좌측으로 조작하면 배경색 역시 투명해지게 되고, 이미지가 투명하고, 
    배경색 역시 투명하다면 저장되는 이미지 자체가 투명하게 저장됩니다.
### 7. 이미지를 저장해보자.
<img src="/Guide images/10.png"/>
    
    저는 이와같이 이미지를 수정했습니다. 이제 저장할 차례입니다.
    우하단에 있는 Save 버튼을 클릭하면 이미지를 저장할 수 있습니다.
<img src="/Guide images/11.png"/>
    
    저장 완료입니다! 
### 8. 한 줄에 들어가는 이미지 수를 바꿔보자.
    이미지 옆에 있는 상자에 한 행에 몇개의 열을 가지는 이미지를 만들 지 설정할 수 있습니다.

    설정한 열의 크기보다 불러온 이미지의 갯수가 적을 경우 자동으로 이미지의 갯수만큼으로 설정합니다.
<img src="/Guide images/12.png"/>

    현재 3장의 이미지가 있으므로 숫자를 2로 변경하고 저장해보겠습니다.
<img src="/Guide images/13.png"/>

    한 줄에 2장이 저장되었습니다. 의도한 대로 되었습니다.
