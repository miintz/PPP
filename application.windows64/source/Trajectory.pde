class Trajectory
{
  public int Index;
  public int Length;
  
 /* 
  * perihelion range for solar objects
  * 
  * pluto (max) ~ 4436 (sounds very 'mumbly')
  * mars (min) ~ 206 (so this sounds crisp)
  *
  * getting perihelion of stars may be a bit confusing, but distance will do nicely in that case 
  * the number is quite arbitrary anyway...
  */
  
  //Determines if the transit is north or south of the observers meridian (NOT USED)
  public boolean North;
  private int Speed;
  public String Name;

  private String[] Dates;

  //RISE
  private String[] RiseAzDate;
  private int[] RizeAzAngle;

  //TRANSIT  
  private String[] TransitAltDate;
  private int[] TransitAltAngle;

  //SET
  private String[] SetAzDate;
  private int[] SetAzAngle;

  Trajectory(String name, int size)
  {
    this.Name = name;
    this.Dates = new String[size];
    this.RiseAzDate = new String[size];
    this.RizeAzAngle = new int[size];
    this.TransitAltDate = new String[size];
    this.TransitAltAngle = new int[size];
    this.SetAzDate = new String[size];
    this.SetAzAngle = new int[size];

    this.Length = size;
  }

  public String getDate()
  {
    return this.Dates[this.Index];
  }

  public String getRiseAzDate()
  {
    return this.RiseAzDate[this.Index];
  }

  public int getRiseAzAngle()
  {
    return this.RizeAzAngle[this.Index];
  }

  public String getTransitAltDate()
  {
    return this.TransitAltDate[this.Index];
  }

  public int getTransitAltAngle()
  {
    return this.TransitAltAngle[this.Index];
  }

  public String getSetAzDate()
  {
    return this.SetAzDate[this.Index];
  }

  public int getSetAzAngle()
  {
    return this.SetAzAngle[this.Index];
  }

  public void setDate(String d)
  {
    this.Dates[this.Index] = d;
  }

  public void setRiseAzDate(String d)
  {
    this.RiseAzDate[this.Index] = d;
  }

  public void setRiseAzAngle(int i)
  {
    this.RizeAzAngle[this.Index] = i;
  }

  public void setTransitAltDate(String d)
  {
    this.TransitAltDate[this.Index] = d;
  }

  public void setTransitAltAngle(int i)
  {
    this.TransitAltAngle[this.Index] = i;
  }

  public void setSetAzDate(String d)
  {
    this.SetAzDate[this.Index] = d;
  }

  public void setSetAzAngle(int i)
  {
    this.SetAzAngle[this.Index] = i;
  }
  
  public void setSpeed(int i)
  {
    //snelheid is afhankelijk van cminute            
    int RiseMinutes = (RiseHour * 60) + RiseMinute;
    int SetMinutes = (SetHour * 60) + SetMinute;
    
    int TotalMinutes = abs(SetMinutes - RiseMinutes);
    
    //speed is het aantal minuten per frame, framerate zelf is niet belangrijk
    int D = TotalMinutes / i; //dit klopt nog niet, ik mis ~ 15 - 25 frames steeds
    
    this.Speed = D;
  }
  
  public int getSpeed()
  {
    return this.Speed;
  }
}

