
//支持中文和字母和数字和下划线
//str验证的字符串,minlength最小长度, maxlength最大长度
function Verification_Chinese_Letter_Number_Underline(str, minlength, maxlength) {
    var reg = /^[a-zA-Z0-9\u4e00-\u9fa5_]+/;
    if (reg.test(str)) {
        if (str.length >= minlength && str.length <= maxlength) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
};
//支持字母和数字和下划线
//str验证的字符串,minlength最小长度, maxlength最大长度
function Verification_Letter_Number_Underline(str, minlength, maxlength) {
    var reg = /^[A-Za-z0-9_]+$/;
    if (reg.test(str)) {
        if (str.length >= minlength && str.length <= maxlength) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
};
//支持字母和数字
//str验证的字符串,minlength最小长度, maxlength最大长度
function Verification_Letter_Number(str, minlength, maxlength) {
    var reg = /^[A-Za-z0-9]+$/;
    if (reg.test(str)) {
        if (str.length >= minlength && str.length <= maxlength) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
};
//支持汉字
//str验证的字符串,minlength最小长度, maxlength最大长度
function Verification_Chinese(str, minlength, maxlength) {
    var reg = /^[\u4e00-\u9fa5]+$/;
    if (reg.test(str)) {
        if (str.length >= minlength && str.length <= maxlength) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
};
//支持字母
//str验证的字符串,minlength最小长度, maxlength最大长度
function Verification_Letter_Number(str, minlength, maxlength) {
    var reg = /^[A-Za-z]+$/;
    if (reg.test(str)) {
        if (str.length >= minlength && str.length <= maxlength) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
};
//支持数字验证(正整数，小数)
//str验证的字符串,minlength最小长度, maxlength最大长度,wholeMaxLength整数部分的最大长度,smallMaxLength小数部分最大的长度
function Verification_Whole_Small_Number(str, minlength, maxlength, wholeMaxLength, smallMaxLength) {
    var reg = /^(?:[1-9][0-9]*|0)(?:.[0-9]+)?$/;
    if (reg.test(str)) {
        if (str.length >= minlength && str.length <= maxlength) {
            var whole, small, split = [];
            split = str.split(".");
            for (var i = 0; i < split.length; i++) {
                if (i == 0) {
                    if (split[i].length > wholeMaxLength) {
                        return false;
                    }
                    if (i == 1) {
                        if (split[i].length > smallMaxLength) {
                            return false;
                        }
                    }
                }
            }
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
};

//支持数字验证(正整数)
//str验证的字符串,minlength最小长度, maxlength最大长度
function Verification_WholeNumber(str, minlength, maxlength) {
    var reg = /^[0-9]*$/;
    if (reg.test(str)) {
        if (str.length >= minlength && str.length <= maxlength) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
};



//邮箱验证
function Verification_Email(strEmail) {
    var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    if (reg.test(strEmail)) {
        return true;
    } else {
        return false;
    }
};
//固定电话验证
//正确格式010-12345678、0912-1234567、(010)-12345678、(0912)1234567、(010)12345678、(0912)-1234567、01012345678、09121234567
function Verification_TelePhone(str_TelePhone) {
    var reg = /^(^0\d{2}-?\d{8}$)|(^0\d{3}-?\d{7}$)|(^\(0\d{2}\)-?\d{8}$)|(^\(0\d{3}\)-?\d{7}$)$/;
    if (reg.test(str_TelePhone)) {
        return true;
    } else {
        return false;
    }
};
//手机号码的验证
//支持13,15,17,14,18开头
function Verification_Phone(str_Phone) {
    var reg = /^(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
    if (reg.test(str_Phone)) {
        return true;
    } else {
        return false;
    }
};
//15和18位的身份证号验证
function Verification_IdCard(idCard) {
    //15位和18位身份证号码的正则表达式
    var regIdCard = /^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$/;

    //如果通过该验证，说明身份证格式正确，但准确性还需计算
    if (regIdCard.test(idCard)) {
        if (idCard.length == 18) {
            var idCardWi = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2); //将前17位加权因子保存在数组里
            var idCardY = new Array(1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2); //这是除以11后，可能产生的11位余数、验证码，也保存成数组
            var idCardWiSum = 0; //用来保存前17位各自乖以加权因子后的总和
            for (var i = 0; i < 17; i++) {
                idCardWiSum += idCard.substring(i, i + 1) * idCardWi[i];
            }
            var idCardMod = idCardWiSum % 11; //计算出校验码所在数组的位置
            var idCardLast = idCard.substring(17); //得到最后一位身份证号码

            //如果等于2，则说明校验码是10，身份证号码最后一位应该是X
            if (idCardMod == 2) {
                if (idCardLast == "X" || idCardLast == "x") {
                    return true;
                } else {
                    return false;
                }
            } else {
                //用计算出的验证码与最后一位身份证号码匹配，如果一致，说明通过，否则是无效的身份证号码
                if (idCardLast == idCardY[idCardMod]) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    } else {
        return false;
    }
}
//Ip地址的验证
function Verification_IP(str_IP) {
    var reg = /^((([1-9]\d?)|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}(([1-9]\d?)|(1\d{2})|(2[0-4]\d)|(25[0-5]))$/;
    if (reg.test(str_IP)) {
        return true;
    } else {
        return false;
    }
};
//Web地址的验证
//只允许http、https、ftp这三种
function Verification_WebIP(str_WebIP) {
    var reg = /^(([hH][tT]{2}[pP][sS]?)|([fF][tT][pP]))\:\/\/[wW]{3}\.[\w-]+\.\w{2,4}(\/.*)?$/;
    if (reg.test(str_WebIP)) {
        return true;
    } else {
        return false;
    }
};
//匹配月份
//"01"～"09"和"1"～"12"。
function Verification_Month(str) {
    var reg = /^(0?[1-9]|1[0-2])$/;
    if (reg.test(str)) {
        return true;
    } else {
        return false;
    }
};