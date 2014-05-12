/*
Navicat MySQL Data Transfer

Source Server         : mysql
Source Server Version : 50616
Source Host           : localhost:3306
Source Database       : loan

Target Server Type    : MYSQL
Target Server Version : 50616
File Encoding         : 65001

Date: 2014-05-12 21:51:20
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `branchcompany`
-- ----------------------------
DROP TABLE IF EXISTS `branchcompany`;
CREATE TABLE `branchcompany` (
  `BranchCompanyID` int(11) NOT NULL AUTO_INCREMENT COMMENT '分公司ID',
  `Name` varchar(50) NOT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `PostCode` varchar(6) DEFAULT NULL,
  `Description` text,
  `Status` int(11) DEFAULT '0' COMMENT '0：正常；\r\n1：禁用；\r\n2：删除；',
  PRIMARY KEY (`BranchCompanyID`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=gb2312;

-- ----------------------------
-- Records of branchcompany
-- ----------------------------
INSERT INTO `branchcompany` VALUES ('1', '北京海淀分公司', '北京市海淀区', '100010', '北京市小额贷款', '0');
INSERT INTO `branchcompany` VALUES ('2', '分公司1', '地址1', '123456', '描述1', '0');
INSERT INTO `branchcompany` VALUES ('3', '分公司2', '地址2', '123456', '描述2', '0');
INSERT INTO `branchcompany` VALUES ('4', '分公司3', '地址3', '123456', '描述3', '1');
INSERT INTO `branchcompany` VALUES ('5', '分公司4', '地址4', '123456', '描述4', '0');
INSERT INTO `branchcompany` VALUES ('6', '分公司5', '地址5', '123456', '描述5', '0');
INSERT INTO `branchcompany` VALUES ('7', '分公司6', '地址6', '123455', '描述6', '0');
INSERT INTO `branchcompany` VALUES ('8', '分公司7', '地址7', '123455', '描述7', '0');
INSERT INTO `branchcompany` VALUES ('9', '分公司8', '地址8', '123456', '描述8', '0');
INSERT INTO `branchcompany` VALUES ('10', '分公司9', '地址9', '123456', '描述9', '0');
INSERT INTO `branchcompany` VALUES ('11', '分公司10', '地址10', '123456', '描述10', '0');
INSERT INTO `branchcompany` VALUES ('12', '分公司11', '地址11', '123456', '描述11', '0');
INSERT INTO `branchcompany` VALUES ('13', '分公司12', '地址12', '123456', '描述12', '0');
INSERT INTO `branchcompany` VALUES ('14', '分公司13', '地址13', '123456', '描述13', '0');
INSERT INTO `branchcompany` VALUES ('15', '分公司14', '地址14', '123456', '地址14', '0');
INSERT INTO `branchcompany` VALUES ('16', '分公司15', '地址15', '123456', '分公司15', '0');
INSERT INTO `branchcompany` VALUES ('17', '分公司16', '地址16', '123456', '分公司16', '0');
INSERT INTO `branchcompany` VALUES ('18', '分公司17', '地址17', '123456', '藐视17', '0');

-- ----------------------------
-- Table structure for `branchcompanyuser`
-- ----------------------------
DROP TABLE IF EXISTS `branchcompanyuser`;
CREATE TABLE `branchcompanyuser` (
  `BranchCompanyUserID` int(11) NOT NULL AUTO_INCREMENT,
  `BranchCompanyID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  PRIMARY KEY (`BranchCompanyUserID`)
) ENGINE=InnoDB DEFAULT CHARSET=gb2312;

-- ----------------------------
-- Records of branchcompanyuser
-- ----------------------------

-- ----------------------------
-- Table structure for `roles`
-- ----------------------------
DROP TABLE IF EXISTS `roles`;
CREATE TABLE `roles` (
  `RoleID` int(11) NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(32) NOT NULL,
  `Status` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`RoleID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of roles
-- ----------------------------
INSERT INTO `roles` VALUES ('1', '管理员', '0');
INSERT INTO `roles` VALUES ('2', '门店经理', '0');
INSERT INTO `roles` VALUES ('3', '客户经理', '0');
INSERT INTO `roles` VALUES ('4', '信申员', '0');
INSERT INTO `roles` VALUES ('5', '信申经理', '0');
INSERT INTO `roles` VALUES ('6', '团队经理', '0');

-- ----------------------------
-- Table structure for `userrole`
-- ----------------------------
DROP TABLE IF EXISTS `userrole`;
CREATE TABLE `userrole` (
  `UserRoleID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `RoleID` int(11) NOT NULL,
  PRIMARY KEY (`UserRoleID`)
) ENGINE=InnoDB DEFAULT CHARSET=gb2312;

-- ----------------------------
-- Records of userrole
-- ----------------------------

-- ----------------------------
-- Table structure for `users`
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `userID` int(11) NOT NULL AUTO_INCREMENT,
  `userName` varchar(32) NOT NULL,
  `password` varchar(32) NOT NULL DEFAULT '123456',
  `realName` varchar(16) DEFAULT NULL,
  `gender` int(11) DEFAULT NULL,
  `employeeNO` varchar(32) DEFAULT NULL,
  `cellphone` varchar(11) DEFAULT NULL,
  `createTime` datetime NOT NULL,
  `updateTime` datetime NOT NULL,
  `status` int(11) NOT NULL DEFAULT '0' COMMENT '0：正常；\r\n1：禁用；\r\n2：删除；',
  PRIMARY KEY (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES ('1', 'pkwblack', 'nihaoma', null, null, null, null, '2014-03-23 14:23:58', '2014-03-23 14:24:09', '0');
