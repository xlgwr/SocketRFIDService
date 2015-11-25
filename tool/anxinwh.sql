/*
Navicat MySQL Data Transfer

Source Server         : 192.168.1.7
Source Server Version : 50611
Source Host           : 192.168.1.7:3306
Source Database       : anxinwh

Target Server Type    : MYSQL
Target Server Version : 50611
File Encoding         : 65001

Date: 2015-11-19 16:13:20
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for m_checkpoint
-- ----------------------------
DROP TABLE IF EXISTS `m_checkpoint`;
CREATE TABLE `m_checkpoint` (
  `checkpointno` varchar(32) CHARACTER SET utf8 NOT NULL,
  `checktime` varchar(8) COLLATE utf8_bin NOT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `status` smallint(6) NOT NULL,
  `adduser` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  `depot_no` varchar(16) CHARACTER SET utf8 NOT NULL,
  PRIMARY KEY (`checkpointno`,`checktime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for m_classinfo
-- ----------------------------
DROP TABLE IF EXISTS `m_classinfo`;
CREATE TABLE `m_classinfo` (
  `cls_no` varchar(16) CHARACTER SET utf8 NOT NULL,
  `infoval` varchar(128) CHARACTER SET utf8 NOT NULL,
  `infoval2` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `infoval3` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `cls_typno` int(4) NOT NULL,
  `sort` int(16) NOT NULL,
  `status` smallint(6) NOT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  PRIMARY KEY (`cls_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for m_depot
-- ----------------------------
DROP TABLE IF EXISTS `m_depot`;
CREATE TABLE `m_depot` (
  `depot_no` varchar(8) CHARACTER SET utf8 NOT NULL,
  `depot_nm` varchar(32) CHARACTER SET utf8 NOT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `status` smallint(6) NOT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`depot_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for m_devicemodel
-- ----------------------------
DROP TABLE IF EXISTS `m_devicemodel`;
CREATE TABLE `m_devicemodel` (
  `modelno` varchar(10) CHARACTER SET utf8 NOT NULL,
  `modenm` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `modeflag` int(11) NOT NULL COMMENT '0：无源信号采集(默认)； 1：有源信号采集',
  `param1` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param2` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param3` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param4` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param5` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param6` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param7` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param8` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param9` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param10` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param11` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param12` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param13` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param14` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param15` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param16` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param17` varchar(16) CHARACTER SET utf8 NOT NULL,
  `param18` varchar(16) CHARACTER SET utf8 NOT NULL,
  `modereamrk` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  `adduser` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`modelno`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for m_devicerelation
-- ----------------------------
DROP TABLE IF EXISTS `m_devicerelation`;
CREATE TABLE `m_devicerelation` (
  `RelationNo` varchar(32) CHARACTER SET utf8 NOT NULL,
  `TerminalNo` varchar(10) CHARACTER SET utf8 NOT NULL,
  `Relation1` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `Relation2` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `Relation3` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `Relation4` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `Relation5` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `Relation6` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `Relation7` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `Relation8` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`RelationNo`,`TerminalNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for m_funcform
-- ----------------------------
DROP TABLE IF EXISTS `m_funcform`;
CREATE TABLE `m_funcform` (
  `formid` varchar(32) CHARACTER SET utf8 NOT NULL,
  `functiontype` varchar(10) CHARACTER SET utf8 NOT NULL,
  `formname` varchar(64) CHARACTER SET utf8 NOT NULL,
  `sortno` int(11) NOT NULL,
  `frmstatus` smallint(6) NOT NULL,
  PRIMARY KEY (`formid`,`functiontype`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for m_parameter
-- ----------------------------
DROP TABLE IF EXISTS `m_parameter`;
CREATE TABLE `m_parameter` (
  `paramkey` varchar(16) NOT NULL,
  `paramvalue` varchar(16) DEFAULT NULL,
  `remark` varchar(256) DEFAULT NULL,
  `paramtype` smallint(6) DEFAULT NULL,
  `adduser` varchar(16) DEFAULT NULL,
  `upduser` varchar(16) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  `depot_no` varchar(16) DEFAULT NULL,
  PRIMARY KEY (`paramkey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for m_products
-- ----------------------------
DROP TABLE IF EXISTS `m_products`;
CREATE TABLE `m_products` (
  `prdct_no` varchar(48) NOT NULL,
  `prdct_nm` varchar(32) NOT NULL,
  `prdct_abbr` varchar(255) NOT NULL,
  `depot_no` varchar(255) NOT NULL,
  `prdct_type` varchar(255) NOT NULL,
  `unit` varchar(255) NOT NULL,
  `remark` varchar(255) DEFAULT NULL,
  `upduser` varchar(255) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  `adduser` varchar(255) DEFAULT NULL,
  `status` smallint(6) NOT NULL,
  PRIMARY KEY (`prdct_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for m_roledetail
-- ----------------------------
DROP TABLE IF EXISTS `m_roledetail`;
CREATE TABLE `m_roledetail` (
  `role_id` varchar(32) NOT NULL COMMENT '自动生成',
  `mod_id` varchar(16) NOT NULL,
  `opr_code` smallint(6) NOT NULL COMMENT '0:新增 1:修改 2:删除 3:查询 4：审核 5：反审核 6：导入 7：导出',
  PRIMARY KEY (`role_id`,`mod_id`,`opr_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for m_roles
-- ----------------------------
DROP TABLE IF EXISTS `m_roles`;
CREATE TABLE `m_roles` (
  `role_id` varchar(32) NOT NULL COMMENT '自动生成',
  `role_nm` varchar(32) NOT NULL,
  `depot_no` varchar(16) NOT NULL,
  `remark` varchar(256) DEFAULT NULL COMMENT '画面输入最大200位',
  `status` smallint(6) NOT NULL COMMENT '默认1:可用 0:不可用',
  `adduser` varchar(16) DEFAULT NULL,
  `upduser` varchar(16) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL COMMENT 'YYYY/MM/dd HH:mm:ss',
  `updtime` datetime DEFAULT NULL COMMENT 'YYYY/MM/dd HH:mm:ss',
  `org_no` varchar(16) DEFAULT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for m_shelf
-- ----------------------------
DROP TABLE IF EXISTS `m_shelf`;
CREATE TABLE `m_shelf` (
  `shelf_no` varchar(16) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `shelf_nm` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `depot_no` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `shelf_type` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `area` varchar(128) CHARACTER SET utf8 DEFAULT NULL,
  `location` varchar(128) CHARACTER SET utf8 DEFAULT NULL,
  `rfidcount` int(11) DEFAULT NULL,
  `remark` varchar(258) CHARACTER SET utf8 DEFAULT NULL,
  `status` smallint(6) DEFAULT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`shelf_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for m_stocking
-- ----------------------------
DROP TABLE IF EXISTS `m_stocking`;
CREATE TABLE `m_stocking` (
  `stock_no` varchar(8) COLLATE utf8_unicode_ci NOT NULL,
  `stock_nm` varchar(32) COLLATE utf8_unicode_ci NOT NULL,
  `remark` varchar(256) COLLATE utf8_unicode_ci DEFAULT NULL,
  `status` smallint(6) NOT NULL,
  `adduser` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `upduser` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`stock_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for m_sysmodule
-- ----------------------------
DROP TABLE IF EXISTS `m_sysmodule`;
CREATE TABLE `m_sysmodule` (
  `mod_id` varchar(16) CHARACTER SET utf8 NOT NULL,
  `mod_nm` varchar(32) CHARACTER SET utf8 NOT NULL,
  `parentid` varchar(16) CHARACTER SET utf8 NOT NULL,
  `url` varchar(256) CHARACTER SET utf8 NOT NULL,
  `iconic` int(16) NOT NULL,
  `islast` int(16) NOT NULL,
  `version` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `flag` smallint(6) NOT NULL,
  `status` smallint(6) NOT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`mod_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for m_sysmoduledetail
-- ----------------------------
DROP TABLE IF EXISTS `m_sysmoduledetail`;
CREATE TABLE `m_sysmoduledetail` (
  `mod_id` varchar(16) CHARACTER SET utf8 NOT NULL,
  `opr_code` smallint(6) NOT NULL,
  `sort` int(16) NOT NULL,
  `status` smallint(6) NOT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`mod_id`,`opr_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for m_terminaldevice
-- ----------------------------
DROP TABLE IF EXISTS `m_terminaldevice`;
CREATE TABLE `m_terminaldevice` (
  `TerminalNo` varchar(10) CHARACTER SET utf8 NOT NULL,
  `ModelNo` varchar(10) CHARACTER SET utf8 NOT NULL,
  `TerminalType` varchar(32) COLLATE utf8_bin NOT NULL,
  `TerminalName` varchar(50) CHARACTER SET utf8 NOT NULL,
  `shelf_no` varchar(96) CHARACTER SET utf8 NOT NULL,
  `ConnectFlag` int(11) NOT NULL COMMENT '0：网口(默认)； 1：串口',
  `SerialNoIPAddr` varchar(32) CHARACTER SET utf8 NOT NULL,
  `ReadTime` int(11) NOT NULL,
  `ReadInterval` int(11) NOT NULL,
  `param1` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param2` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param3` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param4` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param5` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param6` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param7` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param8` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param9` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param10` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param11` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param12` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param13` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param14` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param15` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param16` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param17` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `param18` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `ParamUpdTime` datetime NOT NULL,
  `TrmnUpdTime` datetime DEFAULT NULL,
  `TrmnRemark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `CipherText` varchar(255) CHARACTER SET utf8 NOT NULL,
  `TrmnStatus` smallint(6) NOT NULL COMMENT 'True：启用(默认；False：禁用',
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  `UpdUserNo` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `depot_no` varchar(32) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`TerminalNo`,`ModelNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for m_users
-- ----------------------------
DROP TABLE IF EXISTS `m_users`;
CREATE TABLE `m_users` (
  `user_no` varchar(16) CHARACTER SET utf8 NOT NULL,
  `user_nm` varchar(32) CHARACTER SET utf8 NOT NULL,
  `depot_no` varchar(16) CHARACTER SET utf8 NOT NULL,
  `user_pwd` varchar(64) CHARACTER SET utf8 NOT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `role_id` varchar(128) CHARACTER SET utf8 NOT NULL,
  `status` smallint(6) NOT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  `org_no` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`user_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_alarmdata
-- ----------------------------
DROP TABLE IF EXISTS `t_alarmdata`;
CREATE TABLE `t_alarmdata` (
  `recd_id` varchar(255) NOT NULL,
  `alarm_type` varchar(255) NOT NULL,
  `depot_no` varchar(255) DEFAULT NULL,
  `cell_no` varchar(255) DEFAULT NULL,
  `begin_time` datetime NOT NULL,
  `over_time` datetime DEFAULT NULL,
  `param1` varchar(32) DEFAULT NULL,
  `param2` varchar(32) DEFAULT NULL,
  `param3` varchar(32) DEFAULT NULL,
  `param4` varchar(32) DEFAULT NULL,
  `param5` varchar(32) DEFAULT NULL,
  `remark` varchar(255) DEFAULT NULL,
  `status` smallint(6) NOT NULL,
  `adduser` varchar(255) DEFAULT NULL,
  `upduser` varchar(255) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`recd_id`,`alarm_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_bespeak
-- ----------------------------
DROP TABLE IF EXISTS `t_bespeak`;
CREATE TABLE `t_bespeak` (
  `bespeak_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `bespeak_date` datetime DEFAULT NULL,
  `user_no` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `user_nm` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `custorder` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `contacter` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `tel` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `mobile` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `carrrier` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `pickuser_no` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `pickIdentity` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `tanspotno` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `vendorcode` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `coo` varchar(10) CHARACTER SET utf8 DEFAULT NULL,
  `receiptno` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `status` varchar(6) COLLATE utf8_unicode_ci DEFAULT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`bespeak_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_bespeakdetail
-- ----------------------------
DROP TABLE IF EXISTS `t_bespeakdetail`;
CREATE TABLE `t_bespeakdetail` (
  `bespeak_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `prdct_no` varchar(48) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `pc` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `item_no` varchar(10) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `unit` varchar(10) CHARACTER SET utf8 DEFAULT NULL,
  `quanlity` varchar(20) CHARACTER SET utf8 DEFAULT NULL,
  `pqty` float DEFAULT NULL,
  `qty` float DEFAULT NULL,
  `spec` varchar(10) COLLATE utf8_unicode_ci DEFAULT NULL,
  `nwgt` float DEFAULT NULL,
  `gwgt` float DEFAULT NULL,
  `ctnno` varchar(20) CHARACTER SET utf8 DEFAULT NULL,
  `package` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `status` smallint(6) DEFAULT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`bespeak_no`,`item_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

-- ----------------------------
-- Table structure for t_cash
-- ----------------------------
DROP TABLE IF EXISTS `t_cash`;
CREATE TABLE `t_cash` (
  `cash_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `cash_date` datetime DEFAULT NULL,
  `user_no` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `user_nm` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `custorder` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `contacter` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `tel` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `mobile` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `pickidentity` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `checkcode` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `carrrier` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `tanspotno` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `vendorcode` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `coo` varchar(10) CHARACTER SET utf8 DEFAULT NULL,
  `status` smallint(6) DEFAULT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`cash_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_cashdetail
-- ----------------------------
DROP TABLE IF EXISTS `t_cashdetail`;
CREATE TABLE `t_cashdetail` (
  `cash_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `item_no` varchar(10) CHARACTER SET utf8 NOT NULL,
  `prdct_no` varchar(48) CHARACTER SET utf8 DEFAULT NULL,
  `pc` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `unit` varchar(10) CHARACTER SET utf8 DEFAULT NULL,
  `qty` float DEFAULT NULL,
  `nwgt` float DEFAULT NULL,
  `gwgt` float DEFAULT NULL,
  `quanlity` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `ctnno` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `package` varchar(30) CHARACTER SET utf8 DEFAULT NULL,
  `rfid_no` varchar(96) CHARACTER SET utf8 DEFAULT NULL,
  `receiptNo` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `status` smallint(6) DEFAULT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`cash_no`,`item_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_checkdetailresult
-- ----------------------------
DROP TABLE IF EXISTS `t_checkdetailresult`;
CREATE TABLE `t_checkdetailresult` (
  `check_id` varchar(36) COLLATE utf8_unicode_ci NOT NULL,
  `out_item_no` varchar(10) COLLATE utf8_unicode_ci NOT NULL,
  `prdct_no` varchar(48) COLLATE utf8_unicode_ci NOT NULL,
  `rfid_no` varchar(96) COLLATE utf8_unicode_ci NOT NULL,
  `receiptNo` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `qty` float DEFAULT '0',
  `nwet` float DEFAULT '0',
  `gwet` float DEFAULT '0',
  `cell_no` varchar(30) COLLATE utf8_unicode_ci DEFAULT NULL,
  `remark` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `status` smallint(6) NOT NULL DEFAULT '1',
  `adduser` varchar(16) COLLATE utf8_unicode_ci NOT NULL,
  `upduser` varchar(16) COLLATE utf8_unicode_ci NOT NULL,
  `addtime` datetime NOT NULL,
  `updtime` datetime NOT NULL,
  PRIMARY KEY (`check_id`,`out_item_no`,`prdct_no`,`rfid_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_checkresult
-- ----------------------------
DROP TABLE IF EXISTS `t_checkresult`;
CREATE TABLE `t_checkresult` (
  `check_id` varchar(36) COLLATE utf8_unicode_ci NOT NULL,
  `bespeak_no` varchar(30) COLLATE utf8_unicode_ci DEFAULT NULL,
  `check_date` varchar(16) COLLATE utf8_unicode_ci NOT NULL,
  `user_no` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `user_nm` varchar(32) COLLATE utf8_unicode_ci DEFAULT NULL,
  `status` smallint(6) NOT NULL DEFAULT '1',
  `flag` smallint(6) NOT NULL DEFAULT '0',
  `checktype` smallint(6) NOT NULL DEFAULT '1',
  `remark` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `adduser` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `upduser` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `addtime` datetime NOT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`check_id`),
  KEY `date` (`check_date`,`user_nm`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_functioninfo
-- ----------------------------
DROP TABLE IF EXISTS `t_functioninfo`;
CREATE TABLE `t_functioninfo` (
  `roleid` varchar(10) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `formid` varchar(32) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `rolestatus` smallint(6) NOT NULL,
  PRIMARY KEY (`roleid`,`formid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_interface
-- ----------------------------
DROP TABLE IF EXISTS `t_interface`;
CREATE TABLE `t_interface` (
  `recd_id` int(11) NOT NULL AUTO_INCREMENT,
  `address` varchar(128) DEFAULT NULL,
  `type` int(11) DEFAULT NULL,
  `downtime` datetime DEFAULT NULL,
  `downtype` int(11) DEFAULT NULL,
  `adjunct_address` text,
  `adjunct_value` text,
  `remark` varchar(256) DEFAULT NULL,
  `status` int(11) DEFAULT NULL,
  PRIMARY KEY (`recd_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_roles
-- ----------------------------
DROP TABLE IF EXISTS `t_roles`;
CREATE TABLE `t_roles` (
  `roleid` varchar(10) CHARACTER SET utf8 NOT NULL,
  `rolename` varchar(32) CHARACTER SET utf8 NOT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`roleid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_sampling
-- ----------------------------
DROP TABLE IF EXISTS `t_sampling`;
CREATE TABLE `t_sampling` (
  `bespeak_no` varchar(30) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `stockin_id` varchar(32) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `rfid_no` varchar(96) CHARACTER SET utf8 NOT NULL,
  `qty` float NOT NULL,
  `nwet` float NOT NULL,
  `gwet` float NOT NULL,
  `agwet` float NOT NULL,
  `status` smallint(6) NOT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`stockin_id`,`rfid_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_sensorinfo
-- ----------------------------
DROP TABLE IF EXISTS `t_sensorinfo`;
CREATE TABLE `t_sensorinfo` (
  `depot_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `sensor_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `sensor_name` varchar(30) CHARACTER SET utf8 NOT NULL,
  `sensor_value` float(255,0) NOT NULL,
  `sensor_type` int(255) NOT NULL,
  `status` smallint(255) NOT NULL,
  `remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`depot_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_stock
-- ----------------------------
DROP TABLE IF EXISTS `t_stock`;
CREATE TABLE `t_stock` (
  `prdct_no` varchar(48) NOT NULL,
  `pqty` float NOT NULL DEFAULT '0',
  `qty` float NOT NULL DEFAULT '0',
  `nwet` float NOT NULL DEFAULT '0',
  `gwet` float NOT NULL DEFAULT '0',
  `remark` varchar(256) DEFAULT NULL,
  `status` smallint(6) NOT NULL DEFAULT '1',
  `adduser` varchar(16) DEFAULT NULL,
  `upduser` varchar(16) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`prdct_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_stockdetail
-- ----------------------------
DROP TABLE IF EXISTS `t_stockdetail`;
CREATE TABLE `t_stockdetail` (
  `prdct_no` varchar(48) CHARACTER SET utf8 NOT NULL,
  `rfid_no` varchar(96) CHARACTER SET utf8 NOT NULL,
  `receiptNo` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `shelf_no` varchar(16) CHARACTER SET utf8 NOT NULL,
  `ctnno_no` varchar(30) CHARACTER SET utf8 NOT NULL DEFAULT '0',
  `pqty` float NOT NULL DEFAULT '0',
  `qty` float NOT NULL DEFAULT '0',
  `nwet` float NOT NULL DEFAULT '0',
  `gwet` float NOT NULL DEFAULT '0',
  `remark` varchar(100) CHARACTER SET utf8 NOT NULL,
  `status` smallint(6) NOT NULL DEFAULT '1',
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`prdct_no`,`rfid_no`,`shelf_no`,`ctnno_no`,`remark`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_stockin
-- ----------------------------
DROP TABLE IF EXISTS `t_stockin`;
CREATE TABLE `t_stockin` (
  `stockin_id` varchar(32) NOT NULL,
  `stockin_date` datetime DEFAULT NULL,
  `user_no` varchar(16) DEFAULT NULL,
  `status` smallint(6) DEFAULT '1',
  `op_no` varchar(30) DEFAULT NULL,
  `remark` varchar(256) NOT NULL,
  `adduser` varchar(16) NOT NULL,
  `upduser` varchar(16) NOT NULL,
  `addtime` datetime NOT NULL,
  `updtime` datetime NOT NULL,
  PRIMARY KEY (`stockin_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_stockinctnno
-- ----------------------------
DROP TABLE IF EXISTS `t_stockinctnno`;
CREATE TABLE `t_stockinctnno` (
  `stockin_id` varchar(32) NOT NULL,
  `prdct_no` varchar(48) NOT NULL,
  `pqty` float NOT NULL DEFAULT '0',
  `qty` float NOT NULL DEFAULT '0',
  `nwet` float DEFAULT '0',
  `gwet` float DEFAULT '0',
  `status` smallint(6) NOT NULL DEFAULT '1',
  `adduser` varchar(16) DEFAULT NULL,
  `upduser` varchar(16) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`stockin_id`,`prdct_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_stockinctnnodetail
-- ----------------------------
DROP TABLE IF EXISTS `t_stockinctnnodetail`;
CREATE TABLE `t_stockinctnnodetail` (
  `stockin_id` varchar(32) CHARACTER SET utf8 NOT NULL,
  `item_no` varchar(10) CHARACTER SET utf8 DEFAULT NULL,
  `prdct_no` varchar(48) CHARACTER SET utf8 NOT NULL,
  `rfid_no` varchar(96) CHARACTER SET utf8 NOT NULL,
  `ctnno_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `receiptNo` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `pqty` float NOT NULL DEFAULT '0',
  `qty` float NOT NULL DEFAULT '0',
  `nwet` float DEFAULT '0',
  `gwet` float DEFAULT '0',
  `status` smallint(6) NOT NULL DEFAULT '1',
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`stockin_id`,`prdct_no`,`rfid_no`,`ctnno_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_stockindetail
-- ----------------------------
DROP TABLE IF EXISTS `t_stockindetail`;
CREATE TABLE `t_stockindetail` (
  `stockin_id` varchar(32) NOT NULL,
  `in_item_no` varchar(10) NOT NULL,
  `bespeak_no` varchar(30) NOT NULL,
  `item_no` varchar(10) NOT NULL,
  `prdct_no` varchar(48) DEFAULT NULL,
  `pc` varchar(30) NOT NULL,
  `pqty` float DEFAULT NULL,
  `qty` float NOT NULL,
  `nwet` float NOT NULL,
  `gwet` float NOT NULL,
  `quanlity` varchar(20) NOT NULL,
  `remark` varchar(256) DEFAULT NULL,
  `status` smallint(6) DEFAULT '1',
  `adduser` varchar(16) NOT NULL,
  `upduser` varchar(16) NOT NULL,
  `addtime` datetime NOT NULL,
  `updtime` datetime NOT NULL,
  PRIMARY KEY (`stockin_id`,`in_item_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_stockinsign
-- ----------------------------
DROP TABLE IF EXISTS `t_stockinsign`;
CREATE TABLE `t_stockinsign` (
  `bespeak_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `stockin_id` varchar(32) CHARACTER SET utf8 NOT NULL,
  `user_no` varchar(16) CHARACTER SET utf8 NOT NULL,
  `pickIdentity` varchar(30) CHARACTER SET utf8 NOT NULL,
  `status` smallint(6) NOT NULL,
  `adduser` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `upduser` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`bespeak_no`,`stockin_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_stockout
-- ----------------------------
DROP TABLE IF EXISTS `t_stockout`;
CREATE TABLE `t_stockout` (
  `stockout_id` varchar(32) NOT NULL,
  `stockout_date` datetime DEFAULT NULL,
  `user_no` varchar(16) DEFAULT NULL,
  `pickup_user` varchar(16) DEFAULT NULL,
  `pickup_card` varchar(32) DEFAULT NULL,
  `pickup_mobile` varchar(32) DEFAULT NULL,
  `status` smallint(6) DEFAULT '1',
  `remark` varchar(256) NOT NULL,
  `adduser` varchar(16) NOT NULL,
  `upduser` varchar(16) NOT NULL,
  `addtime` datetime NOT NULL,
  `updtime` datetime NOT NULL,
  PRIMARY KEY (`stockout_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_stockoutctnno
-- ----------------------------
DROP TABLE IF EXISTS `t_stockoutctnno`;
CREATE TABLE `t_stockoutctnno` (
  `stockout_id` varchar(32) NOT NULL,
  `prdct_no` varchar(48) NOT NULL,
  `pqty` float NOT NULL DEFAULT '0',
  `qty` float NOT NULL DEFAULT '0',
  `nwet` float DEFAULT '0',
  `gwet` float DEFAULT '0',
  `status` smallint(6) NOT NULL DEFAULT '1',
  `adduser` varchar(16) DEFAULT NULL,
  `upduser` varchar(16) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`stockout_id`,`prdct_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_stockoutctnnodetail
-- ----------------------------
DROP TABLE IF EXISTS `t_stockoutctnnodetail`;
CREATE TABLE `t_stockoutctnnodetail` (
  `stockout_id` varchar(32) CHARACTER SET utf8 NOT NULL,
  `prdct_no` varchar(48) CHARACTER SET utf8 NOT NULL,
  `rfid_no` varchar(96) CHARACTER SET utf8 NOT NULL,
  `ctnno_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `receiptNo` varchar(50) CHARACTER SET utf8 DEFAULT NULL,
  `pqty` float NOT NULL DEFAULT '0',
  `qty` float NOT NULL DEFAULT '0',
  `nwet` float DEFAULT '0',
  `gwet` float DEFAULT '0',
  `status` smallint(6) NOT NULL DEFAULT '1',
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`stockout_id`,`prdct_no`,`rfid_no`,`ctnno_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_stockoutdetail
-- ----------------------------
DROP TABLE IF EXISTS `t_stockoutdetail`;
CREATE TABLE `t_stockoutdetail` (
  `stockout_id` varchar(32) NOT NULL,
  `out_item_no` varchar(10) NOT NULL,
  `rfid_no` varchar(96) NOT NULL,
  `cash_no` varchar(30) NOT NULL,
  `item_no` varchar(10) NOT NULL,
  `prdct_no` varchar(48) DEFAULT NULL,
  `pc` varchar(30) NOT NULL,
  `receiptNo` varchar(50) DEFAULT NULL,
  `pqty` float DEFAULT NULL,
  `qty` float NOT NULL,
  `nwet` float NOT NULL,
  `gwet` float NOT NULL,
  `quanlity` varchar(20) NOT NULL,
  `status` smallint(1) DEFAULT NULL,
  `adduser` varchar(16) NOT NULL,
  `upduser` varchar(16) NOT NULL,
  `addtime` datetime NOT NULL,
  `updtime` datetime NOT NULL,
  `remark` varchar(256) NOT NULL,
  PRIMARY KEY (`stockout_id`,`out_item_no`,`rfid_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_stockoutsign
-- ----------------------------
DROP TABLE IF EXISTS `t_stockoutsign`;
CREATE TABLE `t_stockoutsign` (
  `cash_no` varchar(30) CHARACTER SET utf8 NOT NULL,
  `cash_code` varchar(32) CHARACTER SET utf8 NOT NULL COMMENT '兑付码',
  `user_no` varchar(16) CHARACTER SET utf8 DEFAULT NULL COMMENT '库仓确认人',
  `pickIdentity` varchar(30) CHARACTER SET utf8 DEFAULT NULL COMMENT '身份证号',
  `status` smallint(6) DEFAULT NULL,
  `adduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `upduser` varchar(16) CHARACTER SET utf8 DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`cash_no`,`cash_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_stockshelves
-- ----------------------------
DROP TABLE IF EXISTS `t_stockshelves`;
CREATE TABLE `t_stockshelves` (
  `stockout_id` varchar(32) COLLATE utf8_unicode_ci NOT NULL,
  `rfid_no` varchar(96) COLLATE utf8_unicode_ci NOT NULL,
  `receiptNo` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `ctnno_no` varchar(30) COLLATE utf8_unicode_ci DEFAULT NULL,
  `shelf_no` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `stocking_no` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `pqty` float DEFAULT NULL,
  `qty` float DEFAULT NULL,
  `nwet` float DEFAULT NULL,
  `gwet` float DEFAULT NULL,
  `status` smallint(6) DEFAULT NULL,
  `adduser` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `upduser` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `updtime` datetime DEFAULT NULL,
  PRIMARY KEY (`stockout_id`,`rfid_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Table structure for t_syslogrecd
-- ----------------------------
DROP TABLE IF EXISTS `t_syslogrecd`;
CREATE TABLE `t_syslogrecd` (
  `log_id` varchar(32) NOT NULL,
  `operatorid` varchar(32) DEFAULT NULL,
  `message` text,
  `type` smallint(6) DEFAULT NULL,
  `result` smallint(6) DEFAULT NULL,
  `mod_id` varchar(50) NOT NULL,
  `adduser` varchar(32) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `org_no` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`log_id`,`mod_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for t_terminaalarm
-- ----------------------------
DROP TABLE IF EXISTS `t_terminaalarm`;
CREATE TABLE `t_terminaalarm` (
  `AlarmNo` varchar(32) CHARACTER SET utf8 NOT NULL,
  `AlarmType` smallint(6) NOT NULL COMMENT '0：采集连接\r\n            1：采集启动\r\n            2：采集扫描',
  `TerminalNo` varchar(10) CHARACTER SET utf8 NOT NULL,
  `AlarmDate` datetime NOT NULL,
  `AlarmFlag` int(11) NOT NULL COMMENT '0: 未处理；1: 已处理\r\n            ',
  `AlarmReason` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `Remark` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `UpdUserNo` varchar(32) CHARACTER SET utf8 DEFAULT NULL,
  `UpdDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AlarmNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
