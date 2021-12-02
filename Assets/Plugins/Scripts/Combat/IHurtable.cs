namespace Thuleanx.Combat {
	public interface IHurtable {
		bool CanTakeHit();
		void ApplyHit(IHit hit);
	}
}