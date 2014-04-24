/*
Navicat MySQL Data Transfer

Source Server         : mysql
Source Server Version : 50616
Source Host           : localhost:3306
Source Database       : loan

Target Server Type    : MYSQL
Target Server Version : 50616
File Encoding         : 65001

Date: 2014-04-23 07:14:11
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=gb2312;

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

-- ----------------------------
-- Table structure for `users`
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `userID` int(11) NOT NULL AUTO_INCREMENT,
  `branchCompanyID` int(11) DEFAULT NULL COMMENT '所属分公司ID',
  `userTypeID` int(11) NOT NULL COMMENT '用户类型ID',
  `userName` varchar(32) NOT NULL,
  `password` varchar(32) NOT NULL,
  `createTime` datetime NOT NULL,
  `updateTime` datetime NOT NULL,
  `status` int(11) NOT NULL DEFAULT '0' COMMENT '0：正常；\r\n1：禁用；\r\n2：删除；',
  PRIMARY KEY (`userID`),
  KEY `userType` (`userTypeID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES ('1', '1', '1', 'pkwblack', 'nihaoma', '2014-03-23 14:23:58', '2014-03-23 14:24:09', '0');

-- ----------------------------
-- Table structure for `usertype`
-- ----------------------------
DROP TABLE IF EXISTS `usertype`;
CREATE TABLE `usertype` (
  `typeID` int(11) NOT NULL AUTO_INCREMENT,
  `typeName` varchar(32) NOT NULL,
  `status` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of usertype
-- ----------------------------
INSERT INTO `usertype` VALUES ('1', '管理员', '0');
INSERT INTO `usertype` VALUES ('2', '门店经理', '0');
INSERT INTO `usertype` VALUES ('3', '客户经理', '0');
INSERT INTO `usertype` VALUES ('4', '信申员', '0');
INSERT INTO `usertype` VALUES ('5', '信申经理', '0');
INSERT INTO `usertype` VALUES ('6', '团队经理', '0');
