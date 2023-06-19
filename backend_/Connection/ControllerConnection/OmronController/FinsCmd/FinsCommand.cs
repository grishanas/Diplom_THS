using backend_.Connection.ControllerConnection.OmronController.TransportLayer;
using Newtonsoft.Json;
using System.Text;
using backend_.Models.controller;

namespace backend_.Connection.ControllerConnection.OmronController.FinsCmd
{
    [Serializable]
	public class FinsRequest
    {
		public string code { get; set; }
			
		public string memoryArea { get; set; }
		public string descriptions { get; set; }
		
		public int startAddress { get; set; }
		
		public byte bitshift { get; set;}

		public int length { get; set; }
    }
	public class FinsCommand : IControllerCommandImplementation
	{
		public int id { get; set; }
		public int Address { get; set; }

		public void SetState(State state)
        {
            lock (this.IsRun)
            {
				this.IsRun.IsRun = state.IsRun;
				this.IsRun.description = state.description;
            }
        }
		public static List<State> AllowedState { get; } = new List<State>()
		{
			new State() { IsRun = true, description = "RUN" },
			new State(){ IsRun = false, description = "STOP" }
		};

		public List<State> GetAllowedState()
        {
			return AllowedState;

		}
		public State IsRun { get; set; }
		#region
		private event CommandListener Answer;
		private event CommandListener Errore;
		public void SetAnswerListener(CommandListener Delegate)
		{
			Answer += Delegate;
		}
		public void DeleteAnswerListener(CommandListener Delegate)
		{
			Answer -= Delegate;
		}


		/// <summary>
		/// не реализовано, здесь осуществляется настройка команды для опроса
		/// </summary>
		/// <param name="comand"></param>
		public FinsCommand(byte[] comand)
		{
			IsRun = new State();
		}
		public FinsCommand(UInt32 address,int id)
		{
			IsRun = new State();
			this.Address = Address;
			this.id = id;
		}



		public void SetCommand(string comand)
		{
			var command = JsonConvert.DeserializeObject<FinsRequest>(comand);
			var code = int.Parse(command.code);
			var memoryArea = (MemoryArea)byte.Parse(command.memoryArea);
			if (code == (ushort)FinsComandCode.MemoryAreaRead)
            {
                MemoryAreaRead((MemoryArea)byte.Parse(command.memoryArea), (UInt16)command.startAddress, (UInt16)command.length, command.bitshift,null);
            }

		}

		public static List<string> allowedCommand = new List<string>()
		{
			JsonConvert.SerializeObject(new FinsRequest()
				{code=((int)FinsComandCode.MemoryAreaRead).ToString(),memoryArea =((int)MemoryArea.CIO_Bit).ToString(),descriptions="чтение байта памяти из CIO",startAddress=0,bitshift=0,length=0}),
			JsonConvert.SerializeObject(new FinsRequest()
				{code=((int)FinsComandCode.MemoryAreaRead).ToString(),memoryArea =((int)MemoryArea.WR).ToString(),descriptions="чтение байта памяти из WR",startAddress=0,bitshift=0,length=0}),
			JsonConvert.SerializeObject(new FinsRequest()
				{code=((int)FinsComandCode.MemoryAreaRead).ToString(),memoryArea =((int)MemoryArea.DM).ToString(),descriptions="чтение слова из области памяти DM",startAddress=0,bitshift=0,length=0}),
			JsonConvert.SerializeObject(new FinsRequest()
				{code=((int)FinsComandCode.MemoryAreaRead).ToString(),memoryArea =((int)MemoryArea.DM_Bit).ToString(),descriptions="чтение бита из области памяти DM",startAddress=0,bitshift=0,length=0}),

		};

		public List<string> GetAllowedCommand()
        {
			return allowedCommand;
        }

		public void SetTransportLaeyr(IControllerConnect client)
        {
			Transport = client;
        }

		private string _lastError = "";
		private IControllerConnect Transport;
		Byte[] cmdFins = new Byte[]
		{
			//---- COMMAND HEADER -------------------------------------------------------
			0x80,				// 00 ICF Information control field 
			0x00,				// 01 RSC Reserved 
			0x02,				// 02 GTC Gateway count
			0x00,				// 03 DNA Destination network address (0=local network)
			0x00,				// 04 DA1 Destination node number
			0x00,				// 05 DA2 Destination unit address
			0x00,				// 06 SNA Source network address (0=local network)
			0x00,				// 07 SA1 Source node number
			0x00,				// 08 SA2 Source unit address
			0x00,				// 09 SID Service ID
			//---- COMMAND --------------------------------------------------------------
			0x00,				// 10 MC Main command
			0x00,				// 11 SC Subcommand
			//---- PARAMS ---------------------------------------------------------------
			0x00,				// 12 reserved area for additional params
			0x00,				// depending on fins command
			0x00,
			0x00,
			0x00,
			0x00,
			0x00,
			0x00,
			0x00,
			0x00,
		};

		byte[] FinsTCPHeadeer = new byte[]
        {
			0x46, 0x49, 0x4E, 0x53,		// 'F' 'I' 'N' 'S'
			0x00, 0x00, 0x00, 0x00,		// Expected number of bytes for response
			0x00, 0x00, 0x00, 0x02,		// Command FS  Sending=2 / Receiving=3
			0x00, 0x00, 0x00, 0x00		// Error code
		};


		UInt16 finsCommandLen = 0;

		const int F_PARAM = 12;


		// FRAME SEND Response array
		//
		byte[] ResponseHeader = new byte[]
		{
			0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00
		};

		// FINS RESPONSE (command)
		//
		Byte[] respFins = new Byte[2048];


		// FINS RESPONSE (data, 2KB reserved memory)
		//
		Byte[] respFinsData = new Byte[2048];


		// response data length
		//
		UInt16 finsResponseLen = 0;

		public TCPClient Client
        {
			get { return Client; }
			set { Client = value; }
        }

		public byte[] ResponseData
        {
			get { return respFinsData;}
        }

		public byte[] ResponceAddress
        {
            get { return new byte[10]; }
        }


		public byte ICF
		{
			get { return cmdFins[0]; }
			set { cmdFins[0] = value; }
		}
		public byte RSV
		{
			get { return this.cmdFins[1]; }
			set { this.cmdFins[1] = value; }
		}
		public byte GCT
		{
			get { return this.cmdFins[2]; }
			set { this.cmdFins[2] = value; }
		}

		public byte DNA
		{
			get { return this.cmdFins[3]; }
			set { this.cmdFins[3] = value; }
		}



		public byte MC
		{
			get { return this.cmdFins[10]; }
			set { this.cmdFins[10] = value; }
		}

		public byte SC
		{
			get { return this.cmdFins[11]; }
			set { this.cmdFins[11] = value; }
		}

		public UInt16 Comand
        {
			get { return (UInt16)(cmdFins[10] >> 8 + cmdFins[11]); }
			set { cmdFins[10] = (byte)(value >> 8 & 0xff); cmdFins[11] = (byte)(value & 0xff); }
        }

		public byte SA1
		{
			get { return cmdFins[7]; }
			set { cmdFins[7] = value; }
		}		
		public byte DA1
		{
			get { return cmdFins[4]; }
			set { cmdFins[4] = value; }
		}

		public int FinsHeaderErrore
        {
			get { return ResponseHeader[8] << 24 + ResponseHeader[9] << 16 + ResponseHeader[10] << 8 + ResponseHeader[11]; }
        }

		public byte FinsResponceMainErroreCode
        {
			get { return respFins[12]; }
        }

		public byte FinsResponceSubErroreCode
		{
			get { return respFins[13]; }
		}


		public UInt16 FinsHeaderLenght
		{
			get { return (UInt16)(FinsTCPHeadeer[6] >> 8 & 0xff + FinsTCPHeadeer[7] & 0xff); }
			set
			{
				this.FinsTCPHeadeer[6] = (byte)((value >> 8) & 0xFF);
				this.FinsTCPHeadeer[7] = (byte)(value & 0xFF);
			}
		}

		public UInt16 FinsResponceLenght
		{
			get
			{
				return (UInt16)(ResponseHeader[6] << 8 + ResponseHeader[7] & 0xff);
			}
		}
        #endregion

        #region FinsComand

        public void MemoryAreaRead(MemoryArea memoryArea,UInt16 StartAddress,UInt16 Count, byte? StartBitPosition, byte[]? data)
        {
			Count = 1;

			this.MC = 0x01;
			this.SC = 0x01;

			this.cmdFins[F_PARAM] = (byte)(memoryArea);

            //memory address
            this.cmdFins[F_PARAM+1] = (byte) (StartAddress >> 8 & 0xff);
            this.cmdFins[F_PARAM+2] = (byte) (StartAddress & 0xff);

			//adress of start bit

			this.cmdFins[F_PARAM + 3] = StartBitPosition!=null ? (byte)StartBitPosition:(byte)0;


			//set count of recive param
            this.cmdFins[F_PARAM +4] = (byte) (Count >> 8 & 0xff);
            this.cmdFins[F_PARAM +5] = (byte) (Count & 0xff);

			this.finsCommandLen = F_PARAM + 6;
		}

		public void Close()
		{
			Transport.Disconect();
		}

        #endregion

        #region DataSend

		public void ConnectToPLC(FinsCommand fins)
        {

			this.SA1 = fins.SA1;
			this.DA1 = fins.DA1;

		}

		private bool ItWasConnect = false;

		public async Task<bool> ConnectToPLC()
        {
			this.Transport.Connect();
			byte[] cmdNADS = new byte[]
			{
			0x46, 0x49, 0x4E, 0x53, // 'F' 'I' 'N' 'S'
			0x00, 0x00, 0x00, 0x0C,	// 12 Bytes expected
			0x00, 0x00, 0x00, 0x00,	// NADS Command (0 Client to server, 1 server to client)
			0x00, 0x00, 0x00, 0x00,	// Error code (Not used)
			0x00, 0x00, 0x00, 0x00  // Client node address, 0 = auto assigned
			};

			await Transport.WriteData(cmdNADS, cmdNADS.Length);

			var resposNADS = new byte[24];

			resposNADS = await Transport.ReadData(24);

			if (resposNADS[15] != 0)
			{
				if (!FinsErrorCodes.ErrorCodes.TryGetValue(resposNADS[15], out string errorDescription))
					errorDescription = "Unknown error";
				this._lastError = "NADS command error: " + resposNADS[15] + "(" + errorDescription + ")";

				ItWasConnect = false;
				return false;
			}
			if (resposNADS[8] != 0 || resposNADS[9] != 0 || resposNADS[10] != 0 || resposNADS[11] != 1)
			{
				this._lastError = "Error sending NADS command. "
									+ resposNADS[8].ToString() + " "
									+ resposNADS[9].ToString() + " "
									+ resposNADS[10].ToString() + " "
									+ resposNADS[11].ToString();
				ItWasConnect = false;
				return false;
			}

			this.SA1 = resposNADS[19];
			this.DA1 = resposNADS[23];


			ItWasConnect = true;
			return true;

		}

		public async Task<bool> ExecuteCommand()
        {
			var run = false;
			lock(this.IsRun)
            {
				if(this.IsRun.IsRun)
                {
					run = true;
                }
            }
			if (run)
			{
				try
				{
					return await this.Simulate();
					if (this.ItWasConnect)
						return await SendFrames(null);
					else
					{
						await this.ConnectToPLC();
					}
				}
				catch (Exception e)
                {
					Console.WriteLine(e);
                }
			}
			return false;

		}

		private async Task<bool> Simulate()
        {
			var random = new Random();
			const string chars = "0123456789";
			var item = random.Next(int.MinValue, int.MaxValue);/* new string(Enumerable.Repeat(chars, 4)
					.Select(s => s[random.Next(s.Length)]).ToArray());*/
			//var data = Encoding.UTF8.GetBytes(item);

			var data = new byte[4];
            data[0] = (byte)item;
            data[1] = (byte)((item >> 8) & 0xff);
            data[2] = (byte)((item >> 16) & 0xff);
            data[3] = (byte)((item >> 24) & 0xff);

            Console.WriteLine(data[0]);
			Console.WriteLine(data[1]);
			Console.WriteLine(data[2]);
			Console.WriteLine(data[3]);

			Console.WriteLine(item);

			var controllerData = new OutputValue() { controllerAddress = (UInt32)this.Address, controllerOutputId = this.id, value = data, DateTime = DateTime.Now };
			if(Answer!=null)
				Answer.Invoke(controllerData);
			return true;
		}

		public string FSR_ERR
		{
			get
			{
				return ResponseHeader[8].ToString()
						+ ResponseHeader[9].ToString()
						+ ResponseHeader[10].ToString()
						+ ResponseHeader[11].ToString();
			}
		}

		protected async Task<bool> SendFrames(byte[]? data)
        {
			Array.Fill(ResponseHeader, (byte)0, 0, ResponseHeader.Length);

			var FinsLength = this.finsCommandLen + 8;

			if(data!=null)
            {
				FinsLength+= data.Length;
			}

			this.FinsHeaderLenght=(UInt16)FinsLength;

			await Transport.WriteData(FinsTCPHeadeer, FinsTCPHeadeer.Length);

			await Transport.WriteData(cmdFins, finsCommandLen);

			if (data != null)
				await Transport.WriteData(data, data.Length);

			this.ResponseHeader = await Transport.ReadData(ResponseHeader.Length);

			if (this.FSR_ERR != "0002")
			{
				this._lastError = "FRAME SEND error: " + this.FSR_ERR;
				return false;
			}

			if (this.ResponseHeader[15] != 0)
			{
				this._lastError = "Error receving FS command: " + this.ResponseHeader[15];
				return false;
			}

			this.finsResponseLen = (ushort)((ResponseHeader[6] << 8 & 0xff) + ResponseHeader[7]) ;
			this.finsResponseLen -= 8;
			this.respFins = await Transport.ReadData(14);


			if(finsResponseLen > 14)
            {
				this.respFinsData = await Transport.ReadData(finsResponseLen - 14);
				var controllerData = new OutputValue() { controllerAddress = (UInt32)this.Address, controllerOutputId = this.id, value = this.respFinsData, DateTime = DateTime.Now };
				await Answer.Invoke(controllerData);
			}

			if (this.FinsResponceMainErroreCode != 0 || this.FinsResponceSubErroreCode != 0)
			{
				this._lastError += string.Format("Response Code error: (Code: {0}  Subcode: {1})",
													this.FinsResponceMainErroreCode, this.FinsResponceSubErroreCode);
				return false;
			}

			
			return true;
        }

        #endregion





    }
}
