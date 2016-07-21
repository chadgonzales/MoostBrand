using Synchronizer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer
{
    class Utils
    {
        public async Task<int> ProcessRequisition()
        {
            Requisitions _repo = new Requisitions();

            //Get unsync from Local
            List<Requisition> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (Requisition _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessRequisitionDetails()
        {
            RequisitionDetails _repo = new RequisitionDetails();

            //Get unsync from Local
            List<RequisitionDetail> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (RequisitionDetail _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessStockTransfer()
        {
            StockTransfers _repo = new StockTransfers();

            //Get unsync from Local
            List<StockTransfer> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (StockTransfer _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessStockTransferDetails()
        {
            StockTransferDetails _repo = new StockTransferDetails();

            //Get unsync from Local
            List<StockTransferDetail> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (StockTransferDetail _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessReceiving()
        {
            Receivings _repo = new Receivings();

            //Get unsync from Local
            List<Receiving> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (Receiving _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessReceivingDetails()
        {
            ReceivingDetails _repo = new ReceivingDetails();

            //Get unsync from Local
            List<ReceivingDetail> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (ReceivingDetail _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessStockAllocation()
        {
            StockAllocations _repo = new StockAllocations();

            //Get unsync from Local
            List<StockAllocation> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (StockAllocation _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessStockAllocationDetails()
        {
            StockAllocationDetails _repo = new StockAllocationDetails();

            //Get unsync from Local
            List<StockAllocationDetail> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (StockAllocationDetail _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessReturn()
        {
            Returns _repo = new Returns();

            //Get unsync from Local
            List<Return> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (Return _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessReturnedItems()
        {
            ReturnedItems _repo = new ReturnedItems();

            //Get unsync from Local
            List<ReturnedItem> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (ReturnedItem _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessStockAdjustment()
        {
            StockAdjustments _repo = new StockAdjustments();

            //Get unsync from Local
            List<StockAdjustment> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (StockAdjustment _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

        public async Task<int> ProcessStockAdjustmentDetails()
        {
            StockAdjustmentDetails _repo = new StockAdjustmentDetails();

            //Get unsync from Local
            List<StockAdjustmentDetail> unsyncList = await _repo.GetUnsyncLocal();

            int _rows = 0;

            foreach (StockAdjustmentDetail _entity in unsyncList)
            {
                //Update Live
                string _result = await _repo.PostLive(_entity);

                if (_result == "ok")
                {
                    //Update local to IsSync = 1
                    await _repo.PostLocal(_entity);

                    _rows++;
                }
            }

            return _rows;
        }

    }
}
