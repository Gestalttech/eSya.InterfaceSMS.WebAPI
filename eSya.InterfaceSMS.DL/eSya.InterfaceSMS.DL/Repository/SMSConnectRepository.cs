﻿using eSya.InterfaceSMS.DL.Entities;
using eSya.InterfaceSMS.DO;
using eSya.InterfaceSMS.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.InterfaceSMS.DL.Repository
{
    public class SMSConnectRepository: ISMSConnectRepository
    {
        private readonly IStringLocalizer<SMSConnectRepository> _localizer;
        public SMSConnectRepository(IStringLocalizer<SMSConnectRepository> localizer)
        {
            _localizer = localizer;
        }
        #region SMS Connect

        public async Task<List<DO_BusinessEntity>> GetActiveEntites()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var locs = db.GtEcbsens.Where(x => x.ActiveStatus)
                        .Select(x => new DO_BusinessEntity
                        {
                            BusinessId = x.BusinessId,
                            BusinessDesc = x.BusinessDesc
                        }).ToListAsync();

                    return await locs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_BusinessLocation>> GetBusinessLocationByBusinessID(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk =db.GtEcpabls.Join
                        (db.GtEcbslns,
                        p=>new {p.BusinessKey},
                        b=>new {b.BusinessKey},
                        (p,b)=> new {p,b})
                        .Where(x=>x.p.ParameterId==2 && x.p.ActiveStatus && x.p.ParmAction
                        && x.b.BusinessId==BusinessId && x.b.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.b.BusinessKey,
                            LocationDescription = r.b.BusinessName + "-" + r.b.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_CountryCodes> GetLocationISDCodeByBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns.Where(x => x.BusinessKey == BusinessKey && x.ActiveStatus)
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_CountryCodes
                        {
                            Isdcode = r.Isdcode,
                            CountryName = r.BusinessName
                        }).FirstOrDefaultAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_SMSConnect>> GetSMSConnectbyBusinessID(int BusinessId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var _bloc = db.GtEcbslns.Where(x => x.BusinessId == BusinessId).FirstOrDefault();
                    int ISDCode = 0;
                    if (_bloc != null)
                    {
                        ISDCode = _bloc.Isdcode;
                    }
                    switch (ISDCode)
                    {
                        case 91:
                            var locs = db.GtEcbslns.Where(x => x.BusinessId == BusinessId)
                        .Join(db.GtEcsm91s,
                         x => x.BusinessKey,
                         y => y.BusinessKey,
                        (x, y) => new DO_SMSConnect
                        {
                            BusinessKey = y.BusinessKey,
                            ServiceProvider = y.ServiceProvider,
                            EffectiveFrom = y.EffectiveFrom,
                            EffectiveTill = y.EffectiveTill,
                            Api = y.Api,
                            UserId = eSyaCryptGeneration.Decrypt(y.UserId),
                            Password = eSyaCryptGeneration.Decrypt(y.Password),
                            SenderId = y.SenderId,
                            ActiveStatus = y.ActiveStatus,
                            ISDCode = 91
                        }).ToListAsync();
                            return await locs;

                        case 254:
                            var result_254 = db.GtEcbslns.Where(x => x.BusinessId == BusinessId)
                        .Join(db.GtEcs254s,
                         x => x.BusinessKey,
                         y => y.BusinessKey,
                        (x, y) => new DO_SMSConnect
                        {
                            BusinessKey = y.BusinessKey,
                            ServiceProvider = y.ServiceProvider,
                            EffectiveFrom = y.EffectiveFrom,
                            EffectiveTill = y.EffectiveTill,
                            Api = y.Api,
                            UserId = eSyaCryptGeneration.Decrypt(y.UserId),
                            Password = eSyaCryptGeneration.Decrypt(y.Password),
                            SenderId = y.SenderId,
                            ActiveStatus = y.ActiveStatus,
                            ISDCode = 254
                        }).ToListAsync();
                            return await result_254;
                        //case 3:
                        //    Console.WriteLine("Wednesday");
                        //    break;

                        default:
                            var defaultlocs = db.GtEcbslns.Where(x => x.BusinessId == BusinessId)
                    .Join(db.GtEcsm91s,
                     x => x.BusinessKey,
                     y => y.BusinessKey,
                    (x, y) => new DO_SMSConnect
                    {
                        BusinessKey = y.BusinessKey,
                        ServiceProvider = y.ServiceProvider,
                        EffectiveFrom = y.EffectiveFrom,
                        EffectiveTill = y.EffectiveTill,
                        Api = y.Api,
                        UserId = eSyaCryptGeneration.Decrypt(y.UserId),
                        Password = eSyaCryptGeneration.Decrypt(y.Password),
                        SenderId = y.SenderId,
                        ActiveStatus = y.ActiveStatus
                    }).ToListAsync();
                            return await defaultlocs;
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateSMSConnect(DO_SMSConnect obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if ( obj.EffectiveFrom.Date >= obj.EffectiveTill.Value.Date)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W00119", Message = string.Format(_localizer[name: "W00119"]) };

                        }
                        //var _bloc = db.GtEcbslns.Where(x => x.BusinessKey == obj.BusinessKey).FirstOrDefault();
                        int ISDCode = obj.ISDCode;
                        switch (ISDCode)
                        {
                            case 91:
                                if (obj.isEdit == 0)
                                {
                                    GtEcsm91 is_effefdateExist = db.GtEcsm91s.FirstOrDefault(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && be.EffectiveFrom >= obj.EffectiveFrom);
                                    if (is_effefdateExist != null)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W00114", Message = string.Format(_localizer[name: "W00114"]) };
                                    }

                                    GtEcsm91 is_effetodateExist = db.GtEcsm91s.FirstOrDefault(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && be.EffectiveTill >= obj.EffectiveTill);
                                    if (is_effetodateExist != null)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W00115", Message = string.Format(_localizer[name: "W00115"]) };

                                    }

                                    var is_SubsCheck = db.GtEcsm91s.FirstOrDefault(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && (be.EffectiveTill >= obj.EffectiveFrom || obj.EffectiveTill >= obj.EffectiveFrom));
                                    if (is_SubsCheck != null)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W00116", Message = string.Format(_localizer[name: "W00116"]) };

                                    }
                                }
                                GtEcsm91 b_Susbs = db.GtEcsm91s.Where(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && be.EffectiveFrom == obj.EffectiveFrom).FirstOrDefault();
                                if (b_Susbs != null)
                                {
                                    b_Susbs.EffectiveTill = obj.EffectiveTill;
                                    b_Susbs.Api = obj.Api;
                                    b_Susbs.UserId = eSyaCryptGeneration.Encrypt(obj.UserId);
                                    b_Susbs.Password = eSyaCryptGeneration.Encrypt(obj.Password);
                                    b_Susbs.SenderId = obj.SenderId;
                                    b_Susbs.ActiveStatus = obj.ActiveStatus;
                                    b_Susbs.ModifiedBy = obj.User_ID;
                                    b_Susbs.ModifiedOn = System.DateTime.Now;
                                    b_Susbs.ModifiedTerminal = obj.TerminalID;
                                    await db.SaveChangesAsync();
                                    dbContext.Commit();
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                                }
                                else
                                {
                                    var b_Subs = new GtEcsm91
                                    {
                                        BusinessKey = obj.BusinessKey,
                                        ServiceProvider = obj.ServiceProvider,
                                        EffectiveFrom = obj.EffectiveFrom,
                                        EffectiveTill = obj.EffectiveTill,
                                        Api = obj.Api,
                                        UserId = eSyaCryptGeneration.Encrypt(obj.UserId),
                                        Password = eSyaCryptGeneration.Encrypt(obj.Password),
                                        SenderId = obj.SenderId,
                                        ActiveStatus = obj.ActiveStatus,
                                        FormId = obj.FormID,
                                        CreatedBy = obj.User_ID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID
                                    };

                                    db.GtEcsm91s.Add(b_Subs);
                                    await db.SaveChangesAsync();
                                    dbContext.Commit();
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                                }


                            case 254:
                                if (obj.isEdit == 0)
                                {
                                    GtEcs254 is_effefdateExist = db.GtEcs254s.FirstOrDefault(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && be.EffectiveFrom >= obj.EffectiveFrom);
                                    if (is_effefdateExist != null)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W00114", Message = string.Format(_localizer[name: "W00114"]) };
                                    }

                                    GtEcs254 is_effetodateExist = db.GtEcs254s.FirstOrDefault(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && be.EffectiveTill >= obj.EffectiveTill);
                                    if (is_effetodateExist != null)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W00115", Message = string.Format(_localizer[name: "W00115"]) };

                                    }

                                    var is_SubsCheck = db.GtEcs254s.FirstOrDefault(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && (be.EffectiveTill >= obj.EffectiveFrom || obj.EffectiveTill >= obj.EffectiveFrom));
                                    if (is_SubsCheck != null)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W00116", Message = string.Format(_localizer[name: "W00116"]) };

                                    }
                                }
                                GtEcs254 sms_conn = db.GtEcs254s.Where(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && be.EffectiveFrom == obj.EffectiveFrom).FirstOrDefault();
                                if (sms_conn != null)
                                {
                                    sms_conn.EffectiveTill = obj.EffectiveTill;
                                    sms_conn.Api = obj.Api;
                                    sms_conn.UserId = eSyaCryptGeneration.Encrypt(obj.UserId);
                                    sms_conn.Password = eSyaCryptGeneration.Encrypt(obj.Password);
                                    sms_conn.SenderId = obj.SenderId;
                                    sms_conn.ActiveStatus = obj.ActiveStatus;
                                    sms_conn.ModifiedBy = obj.User_ID;
                                    sms_conn.ModifiedOn = System.DateTime.Now;
                                    sms_conn.ModifiedTerminal = obj.TerminalID;
                                    await db.SaveChangesAsync();
                                    dbContext.Commit();
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                                }
                                else
                                {
                                    var b_Subs = new GtEcs254
                                    {
                                        BusinessKey = obj.BusinessKey,
                                        EffectiveFrom = obj.EffectiveFrom,
                                        EffectiveTill = obj.EffectiveTill,
                                        Api = obj.Api,
                                        UserId = eSyaCryptGeneration.Encrypt(obj.UserId),
                                        Password = eSyaCryptGeneration.Encrypt(obj.Password),
                                        SenderId = obj.SenderId,
                                        ActiveStatus = obj.ActiveStatus,
                                        FormId = obj.FormID,
                                        CreatedBy = obj.User_ID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID
                                    };

                                    db.GtEcs254s.Add(b_Subs);
                                    await db.SaveChangesAsync();
                                    dbContext.Commit();
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                                }

                            default:
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W00117", Message = string.Format(_localizer[name: "W00117"]) };
                        }

                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> ActiveOrDeActiveSMSConnect(DO_SMSConnect obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        int ISDCode = obj.ISDCode;
                        switch (ISDCode)
                        {
                            case 91:

                                GtEcsm91 smscon = db.GtEcsm91s.Where(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && be.EffectiveFrom.Date == obj.EffectiveFrom.Date).FirstOrDefault();
                                if (smscon == null)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W00118", Message = string.Format(_localizer[name: "W00118"]) };
                                }

                                smscon.ActiveStatus = obj.status;
                                await db.SaveChangesAsync();
                                dbContext.Commit();

                                if (obj.status == true)
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                                else
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };


                            case 254:

                                GtEcs254 sms = db.GtEcs254s.Where(be => be.BusinessKey == obj.BusinessKey && be.ServiceProvider.ToUpper().Replace(" ", "") == obj.ServiceProvider.ToUpper().Replace(" ", "") && be.EffectiveFrom == obj.EffectiveFrom).FirstOrDefault();
                                if (sms == null)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W00118", Message = string.Format(_localizer[name: "W00118"]) };
                                }

                                sms.ActiveStatus = obj.status;
                                await db.SaveChangesAsync();
                                dbContext.Commit();

                                if (obj.status == true)
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                                else
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };

                            default:
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W00167", Message = string.Format(_localizer[name: "W00167"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
              
            }
        }

        #endregion
    }
}
