/*
Navicat MySQL Data Transfer

Source Server         : mysql
Source Server Version : 50616
Source Host           : localhost:3306
Source Database       : loan

Target Server Type    : MYSQL
Target Server Version : 50616
File Encoding         : 65001

Date: 2014-03-30 19:39:55
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `users`
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `userID` int(11) NOT NULL AUTO_INCREMENT,
  `userTypeID` int(11) NOT NULL,
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
INSERT INTO `users` VALUES ('1', '1', 'pkwblack', 'nihaoma', '2014-03-23 14:23:58', '2014-03-23 14:24:09', '0');

-- ----------------------------
-- Table structure for `usertype`
-- ----------------------------
DROP TABLE IF EXISTS `usertype`;
CREATE TABLE `usertype` (
  `typeID` int(11) NOT NULL AUTO_INCREMENT,
  `typeName` varchar(32) NOT NULL,
  `status` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of usertype
-- ----------------------------
INSERT INTO `usertype` VALUES ('1', '管理员', '0');
INSERT INTO `usertype` VALUES ('2', '门店经理', '0');
INSERT INTO `usertype` VALUES ('3', '业务员', '0');
INSERT INTO `usertype` VALUES ('4', '信申人员', '0');
INSERT INTO `usertype` VALUES ('5', '信申经理', '0');
